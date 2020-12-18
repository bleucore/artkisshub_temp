using System;

namespace MakarovDev.ExpandCollapsePanel
{
    /// <summary>
    /// Информация о развёртывании/свёртывании контрола
    /// </summary>
    public class RefreshEventArgs : EventArgs
    {
        /// <summary>
        /// true - контрол развёрнут. false - контрол свёрнут
        /// </summary>
        public bool IsExpanded { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="isExpanded">true - контрол развёрнут. false - контрол свёрнут</param>
        public RefreshEventArgs(bool isExpanded)
        {
            IsExpanded = isExpanded;
        }
    }
}
