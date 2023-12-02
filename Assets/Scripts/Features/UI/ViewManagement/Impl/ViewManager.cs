using Features.UI.Presenters;
using Features.UI.Presenters.Impl;
using Features.UI.Views;
using UnityEngine;
using Zenject;

namespace Features.UI.ViewManagement.Impl
{
    public class ViewManager : IViewManager
    {
        [Inject]
        private ViewFactory _factory;
        
        public void OpenFailView()
        {
            OpenView<IFailPopupView, FailPopupPresenter>(true);
        }
        
        private void OpenView<TView, TPresenter>(bool useBackground = false) where TView : IView where TPresenter : class, IPresenter
        {
            _factory.CreateView<TView, TPresenter>(useBackground);
        }
    }
}