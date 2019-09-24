using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using osu_mapping;
using osu_mapping.objects;

namespace Annex_Patcher.managers
{
    /// <summary>
    /// PlaceHolderHandler handles rewriting placeholder classes and methods in the CIL
    /// </summary>
    public class PlaceholderHandler
    {
        /// <summary>
        /// Rewrites placeholder classes and methods in the CIL
        /// </summary>
        public void ReplacePlaceholders()
        {
            //Search for Annex classes
            var annexClasses = Patcher.osu_exe.GetTypes().Where(IsNotAnnexPlaceholder);
            var annexMethods = annexClasses.SelectMany(type => type.Methods).ToList();

            foreach (var annexClass in annexClasses)
                HandleClassBase(annexClass);

            foreach (var annexMethod in annexMethods)
                ReplaceSigs(annexMethod);

            //Go through every Annex-related class method
            foreach (var annexMethod in annexMethods)
            {
                SearchAnnexMethod(annexMethod);
            }

            // SetMethodsPublic();
        }

        private void ReplaceSigs(MethodDef methodDef)
        {
            var parems = methodDef.Parameters;
            for (var i = 0; i < methodDef.Parameters.Count; i++)
                if (parems[i].Type.FullName.Contains("Annex.placeholder"))
                {
                    var cleanName = parems[i].Type.TypeName.Replace("&", "");
                    var dirtyName = parems[i].Type.TypeName;

                    Console.WriteLine("Clean: " + dirtyName);

                    var classObj = MappingManager.GetClass(cleanName);

                    if (!classObj.IsPresent())
                    {
                        Console.WriteLine("!! [Replace] Class obj not found");
                        continue;
                    }

                    var actual = Patcher.osu_exe.Find(classObj.Get().ObfName, false);

                    Console.WriteLine(">> " + parems[i].Type.FullName);

                    //Console.WriteLine(parems[i].ParamDef.FullName);

                    //Console.WriteLine(parems[i].Type.Is);
                    parems[i].Type = actual.ToTypeSig();
                    //parems[i].ParamDef.Attributes 
                    //.WriteLine(parems[i].ParamDef.);

                    // parems[i] = actual.ToTypeSig();
//                        parems[i].TryGetTypeDef() = ElementType.FnPtr;


                    //Console.WriteLine(">> " + parems[i].TypeName);
                }
        }

        //Ensures the type is a class of Annex but isn't a placeholder class
        private bool IsNotAnnexPlaceholder(TypeDef type) =>
            type.FullName.StartsWith("Annex") && !type.FullName.StartsWith("Annex.placeholder");

        private void HandleClassBase(TypeDef typeDef)
        {
            var baseType = typeDef.BaseType;

            if (baseType == null || !baseType.FullName.Contains("Annex.placeholder")) return;
            var classObjOpt = MappingManager.GetClass(baseType.Name);

            if (!classObjOpt.IsPresent())
            {
                Console.WriteLine("!! Class Object is missing for: " + baseType.Name);
                return;
            }

            typeDef.BaseType = Patcher.osu_exe.Find(classObjOpt.Get().ObfName, false);
        }

        /// <summary>
        /// Searches the MethodDef's instruction list for method calls containing placeholder classes then replaces them
        /// </summary>
        /// <param name="methodDef">The MethodDef to search through</param>
        private void SearchAnnexMethod(MethodDef methodDef)
        {
            //List of all instructions that are a MethodDef
            var methodInstructions = methodDef.Body?.Instructions;

            if (methodInstructions == null) return;

            var methodInsn = methodInstructions.ToList();

            foreach (var instruction in methodInsn)
            {
                if (!instruction.ToString().Contains("Annex.placeholder") &&
                    !instruction.ToString().Contains("#=")) continue;

                var obfuscatedName = instruction.ToString().Contains("#=");
                var methodOpcode = instruction.OpCode;
                var insnOperand = instruction.Operand;

                if (insnOperand is FieldDef)
                {
                    HandleLdfld(instruction);
                }
                else if (insnOperand is MethodDef)
                {
                    var className = GetClassName(insnOperand);
                    MappingManager.GetClass(className, IsObfuscatedName(className))
                        .IfPresent(classObj =>
                        {
                            var actualClass = GetClass(classObj.ObfName);
                            if (insnOperand == OpCodes.Newobj)
                                HandleCtor(actualClass, instruction);
                            else
                                HandleGeneralMethodCall(actualClass, instruction, classObj);
                        })
                        .Otherwise(() =>
                            Console.WriteLine(
                                $"[!!][MethodDef] Class not found... {className} in {methodDef.DeclaringType.FullName}"));
                }
                else if (insnOperand is TypeDef)
                {
                    var className = GetClassName(insnOperand);
                    MappingManager.GetClass(className, IsObfuscatedName(className))
                        .IfPresent(classObj =>
                        {
                            instruction.Operand = OpCodes.Isinst.ToInstruction(GetClass(classObj.ObfName)).Operand;
                        })
                        .Otherwise(() => Console.WriteLine($"[!!][TypeDef] Class not found... {className}"));
                }
            }
        }

        private void HandleStfld(TypeDef actualClass, Instruction instruction)
        {
            var fieldOperand = (FieldDef) instruction.Operand;
            fieldOperand.FieldType = actualClass.ToTypeSig();
            if (!fieldOperand.DeclaringType.FullName.Contains("Annex.placeholder")) return;

            var declaring = fieldOperand.DeclaringType;
            var declaringOpt = MappingManager.GetClass(declaring.Name);

            if (!declaringOpt.IsPresent())
            {
                Console.WriteLine("!! [HandleStfld] Declaring Option doesn't exist! " + declaring.Name);
                return;
            }

            var delcaringObj = declaringOpt.Get();

            var declaringClass = Patcher.osu_exe.Find(delcaringObj.ObfName, false);

            fieldOperand.DeclaringType = declaringClass;
            fieldOperand.Name = delcaringObj.GetField(fieldOperand.Name).Get().ObfName;
        }

        /// <summary>
        /// Handles replacing code for most method calling OpCodes
        /// </summary>
        /// <param name="actualClass">instance</param>
        /// <param name="instruction">instance </param>
        /// <param name="classObj">Class Object </param>
        private void HandleGeneralMethodCall(TypeDef actualClass, Instruction instruction, ClassObj classObj)
        {
            var operandCast = (MethodDef) instruction.Operand;
            var methodName = operandCast.Name;

            if (methodName == ".ctor")
            {
                HandleCtor(actualClass, instruction);
                return;
            }

            var methodObjOpt = classObj.GetMethod(methodName);

            if (!methodObjOpt.IsPresent())
            {
                Console.WriteLine("!! [Genera] Method Object is missing for: " + actualClass.Name + "." +
                                  operandCast.Name);
                return;
            }

            var methodObj = methodObjOpt.Get();
            //Todo: Make this use a MethodSig for finding the method
            //Finds the obfuscated class' method's MethodDef
            var actualMethod = actualClass.FindMethod(methodObj.ObfName);
            //Sets the instructions operand to our new one
            instruction.Operand = instruction.OpCode.ToInstruction(actualMethod).Operand;
        }

        private void HandleLdfld(Instruction instruction)
        {
            var fieldOperand = (FieldDef) instruction.Operand;

            var fieldName = fieldOperand.Name;
            var declaringName = fieldOperand.DeclaringType.FullName;
            var fieldTypeName = fieldOperand.FieldType.FullName;

            //Replaces the field's type with the obfuscated class name
            if (IsPlaceholder(fieldTypeName) || IsObfuscatedName(fieldTypeName))
            {
                var typeName = fieldOperand.FieldType.TypeName;
                MappingManager.GetClass(typeName, IsObfuscatedName(typeName))
                    .IfPresent(classObj => fieldOperand.FieldType = GetClass(classObj.ObfName).ToTypeSig())
                    .Otherwise(
                        () => Console.WriteLine($"[!!] FieldType not found... {fieldOperand.FieldType.TypeName}"));
            }

            //Replaces the field's declaring class. Aka we extend and the field is in the paren't class.
            if (!IsPlaceholder(declaringName) && !IsObfuscatedName(declaringName)) return;

            var declaringTypeName = fieldOperand.DeclaringType.Name;
            MappingManager.GetClass(declaringTypeName, IsObfuscatedName(declaringTypeName))
                .IfPresent(classObj =>
                {
                    fieldOperand.DeclaringType = GetClass(classObj.ObfName);

                    //If the declaring type is a placeholder class then the field name must also be
                    classObj.GetField(fieldName.String)
                        .IfPresent(fieldObj =>
                        {
                            var path = Patcher.osu_exe.Find(classObj.ObfName, false).FindField(fieldObj.ObfName);
                            instruction.Operand = instruction.OpCode.ToInstruction(path).Operand;
                        })
                        .Otherwise(() => Console.WriteLine($"[!!] FieldName not found... {fieldName}"));
                })
                .Otherwise(() =>
                    Console.WriteLine($"[!!] DeclaringType not found... {fieldOperand.DeclaringType.Name}"));
        }

        private static bool IsObfuscatedName(string s) => s.StartsWith("#=");
        private static TypeDef GetClass(string s) => Patcher.osu_exe.Find(s, false);
        private static bool IsPlaceholder(string s) => s.StartsWith("Annex.placeholder");

        /// <summary>
        /// Replaces code for method calls that are constructor-related
        /// </summary>
        /// <param name="actualClass"></param>
        /// <param name="instruction"></param>
        private void HandleCtor(TypeDef actualClass, Instruction instruction)
        {
            var methodCall = instruction.Operand as MethodDef;

            HandleMethodSig(methodCall.MethodSig);

            var actualCtor = actualClass.FindMethod(".ctor", methodCall.MethodSig);
            //Sets the instructions operand to our new one
            instruction.Operand = instruction.OpCode.ToInstruction(actualCtor).Operand;

            if (instruction.Operand != null) return;

            //  Console.WriteLine(methodCall.FullName);
            //  Console.WriteLine(actualCtor);
        }

        /// <summary>
        /// Gets the class name from an operand.
        /// eg: the class of a field's type, the class a method belongs to
        /// </summary>
        /// <param name="operand">Instruction Operand</param>
        /// <param name="fullName">If true return the full name of the operand</param>
        /// <returns>Returns the class name</returns>
        private string GetClassName(object operand, bool fullName = false)
        {
            switch (operand)
            {
                case MethodDef def:
                    return fullName ? def.FullName : def.DeclaringType.Name.String;
                case FieldDef def:
                    return fullName ? def.FullName : def.FieldType.TypeName;
                case TypeDef def:
                    return fullName ? def.FullName : def.Name.String;
                default:
                    return "";
            }
        }

        private void HandleMethodSig(MethodSig methodSig)
        {
            for (var index = 0; index < methodSig.Params.Count; index++)
            {
                var methodSigParam = methodSig.Params[index];
                if (!methodSigParam.FullName.StartsWith("Annex.placeholder")) continue;

                var typeName = methodSigParam.TypeName;
                var classObjOpt = MappingManager.GetClass(typeName);
                var classObj = classObjOpt.Get();

                var actualClass = Patcher.osu_exe.Find(classObj.ObfName, false);

                methodSig.Params[index] = actualClass.ToTypeSig();
            }
        }

        private void SetMethodsPublic()
        {
            var classes = Patcher.osu_exe.GetTypes();
            var mappedClasses = classes.Where(t => MappingManager.GetClass(t.Name, true).IsPresent());

            foreach (var mappedClass in mappedClasses)
            {
                var classObj = MappingManager.GetClass(mappedClass.Name, true).Get();
                var mappedMethods = mappedClass.Methods.Where(m => classObj.GetMethod(m.Name, true).IsPresent());

                foreach (var mappedMethod in mappedMethods)
                {
                    var methodObj = classObj.GetMethod(mappedMethod.Name, true).Get();
                    if ((mappedMethod.Access & MethodAttributes.Public) != MethodAttributes.Public)
                        mappedMethod.Access = MethodAttributes.Public;
                }
            }
        }
    }
}