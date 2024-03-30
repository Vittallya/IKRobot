using Assets.Scripts.Common.Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Common.Components
{
    public enum CurrentGlobalMode
    {
        /// <summary>
        /// свободное перемещние по сцене
        /// </summary>
        Free,
        /// <summary>
        /// режим добавления или редактирования маршрута по точкам
        /// </summary>
        RouteWorking,
        /// <summary>
        /// воспроизведение движения по маршруту
        /// </summary>
        RoutePlaying,
        /// <summary>
        /// текущее воспроизведение приостановлено
        /// </summary>
        RoutePlayingPaused,

        /// <summary>
        /// прямое управление роботом
        /// </summary>
        RealTimeMoving
    }

    public class UIConfigComponent : MonoBehaviour
    {
        private List<Route> routes = new();
        public CurrentGlobalMode CurrentMode = CurrentGlobalMode.Free;

        public Canvas Canvas;

        public KeyCode UIVisibleToggle = KeyCode.U;

        private void Update()
        {
            if (Input.GetKeyUp(UIVisibleToggle))
            {
                Canvas.gameObject.SetActive(!Canvas.gameObject.active);
            }
        }

        public void SetX(Canvas canvas)
        {

        }
        public void SetY(Canvas canvas)
        {

        }
        public void SetZ(Canvas canvas)
        {

        }

        public void OnAddRoute()
        {
            
        }

    }
}
