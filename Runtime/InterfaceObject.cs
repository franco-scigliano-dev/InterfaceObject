using System;
using UnityEngine;

namespace com.fscigliano.InterfaceObject
{
    /// <summary>
    /// Creation Date:   10/4/2020 1:27:19 PM
    /// Product Name:    Interface Object
    /// Developers:      Franco Scigliano
    /// Description:     
    /// </summary>
    [System.Serializable]
    public class InterfaceObject<T>:InterfaceObjectBase where T : class
    {
        #region Inspector Fields
        [SerializeField] protected UnityEngine.Object _data;
        protected T data;
        #endregion

        #region Properties, Consts and Statics
        public override Type FilterType
        {
            get => typeof(T);
        }
        public T Value
        {
            get
            {
                if (_changed || data == null)
                {
                    data = _data as T;
                    _changed = false;
                }
                return data;
            }
            set
            {
                data = value;
                _data = data as UnityEngine.Object;
                _changed = false;
            }

        }

        #endregion
    }
}