namespace Annex.eventmanager.imp
{
    public interface IEvent
    {
        /// <summary>
        /// Called after the event has been fired to preform actions afterwards
        /// </summary>
        /// <param name="inst">Instance of the class firing the event</param>
        void PostFire(object inst);
    }
}