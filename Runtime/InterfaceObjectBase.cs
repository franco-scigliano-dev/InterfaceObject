using System;

namespace com.fscigliano.InterfaceObject
{
    /// <summary>
    /// Creation Date:   10/4/2020 2:33:28 PM
    /// Product Name:    Interface Object
    /// Developers:      Franco Scigliano
    /// Description:     
    /// </summary>
    public abstract class InterfaceObjectBase
    {
        #region Properties, Consts and Statics
        public virtual Type FilterType
        {
            get;
        }
        public bool Changed
        {
            set => _changed = true;
        }
        #endregion

        #region Variables
        protected bool _changed = false;
        #endregion
    }
}