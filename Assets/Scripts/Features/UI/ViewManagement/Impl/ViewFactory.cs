using System;
using Features.UI.Presenters;
using Features.UI.Views;
using Features.UI.Views.Impl;
using UnityEngine;
using Zenject;

namespace Features.UI.ViewManagement.Impl
{
    public class ViewFactory : MonoBehaviour
    {
        [Inject]
        private readonly DiContainer _container;
        
        [SerializeField] 
        private Transform _darkBackGround;
        [SerializeField]
        private Transform _popupsContainer;
        [SerializeField]
        private FailPopupView _failPopup;
        [SerializeField]
        private LevelPopupView _levelPopup;
        
        public TView CreateView<TView, TPresenter>(bool useBackground = false)
            where TView : IView
            where TPresenter : class, IPresenter
        {
            var viewPrefab = GetPrefabForView<TView>();
            var view = _container.InstantiatePrefabForComponent<TView>(viewPrefab, _popupsContainer);
            if (useBackground)
            {
                _darkBackGround.gameObject.SetActive(true);
            }
            
            var presenter = _container.Instantiate<TPresenter>(new object[] { view });
            presenter.Initialize();
            view.ViewDestroyed += () =>
            {
                if (_darkBackGround.gameObject.activeSelf)
                {
                    _darkBackGround.gameObject.SetActive(false);
                }
                presenter.Dispose();
            };

            return view;
        }
        
        //TODO: Refactor
        private GameObject GetPrefabForView<TView>() where TView : IView
        {
            var viewType = typeof(TView);

            return viewType.Name switch
            {
                nameof(IFailPopupView) => _failPopup.gameObject,
                nameof(ILevelPopupView) => _levelPopup.gameObject,
                _ => throw new InvalidOperationException("No prefab defined for view type: " + viewType.Name)
            };
        }
    }
}