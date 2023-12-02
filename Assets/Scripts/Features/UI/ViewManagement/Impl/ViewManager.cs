using Features.UI.Presenters;
using Features.UI.Presenters.Impl;
using Features.UI.Views;
using Zenject;

namespace Features.UI.ViewManagement.Impl
{
    public class ViewManager : IViewManager
    {
        [Inject]
        private ViewFactory _factory;
        
        public void OpenLevelView(int level)
        {
            var view = OpenView<ILevelPopupView, LevelPopupPresenter>(true);
            view.SetData(level);
        }
        
        public void OpenFailView()
        {
            OpenView<IFailPopupView, FailPopupPresenter>(true);
        }
        
        private TView OpenView<TView, TPresenter>(bool useBackground = false) where TView : IView where TPresenter : class, IPresenter
        {
            return _factory.CreateView<TView, TPresenter>(useBackground);
        }
    }
}