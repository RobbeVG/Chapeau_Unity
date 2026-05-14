using Reflex.Attributes;
using Seacore.Common.Services;
using UnityEngine;
using UnityEngine.UI;
using Seacore.Common;
using DG.Tweening;


namespace Seacore.Game
{
    public class UIStartController : MonoBehaviour
    {
        [Inject]
        WindowManager windowManager;

        [SerializeField]
        Button playButton;
        [SerializeField]
        Button quitButton;

        [SerializeField]
        GameObject playNetworkTypeButtons;

        [SerializeField]
        Button localButton;

        [SerializeField]
        Window playerMenu = null;

        private void Awake()
        {
            quitButton.onClick.AddListener(Reflex.Core.Container.RootContainer.Single<QuitService>().QuitApplication);
            playNetworkTypeButtons.transform.localScale = Vector3.zero;
            AddPlayButtonAnimation();
            localButton.onClick.AddListener(() => {
                windowManager.OpenWindow(playerMenu);
            });
        }

        private void AddPlayButtonAnimation()
        {
            playButton.onClick.AddListener(() => {
                LayoutElement layoutelement = playNetworkTypeButtons.GetComponent<LayoutElement>();
                Sequence sequence = DOTween.Sequence();
                sequence
                .Append(playButton.transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.InExpo))
                .AppendInterval(0.05f)
                .Join(playNetworkTypeButtons.transform.DOScale(Vector3.one, 0.35f).SetEase(Ease.InExpo))
                .OnStart(() => { playNetworkTypeButtons.SetActive(true); })
                .OnComplete(() => {
                    playButton.gameObject.SetActive(false);
                    layoutelement.ignoreLayout = false;
                })
                .Play();
            });
        }
    }
}
