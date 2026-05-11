using Reflex.Attributes;
using Seacore.Common.Services;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Seacore.Common;
using DG.Tweening;


namespace Seacore.Game
{
    public class UIStartController : MonoBehaviour
    {
        [Inject]
        GameRoundManager gameRoundManager;

        [SerializeField]
        Button playButton;
        [SerializeField]
        Button quitButton;

        [SerializeField]
        GameObject playTypeButtons;

        [SerializeField]
        Button localButton;

        [SerializeField]
        GameObject PlayerAmountButtons;


        private void Awake()
        {
            quitButton.onClick.AddListener(Reflex.Core.Container.RootContainer.Single<QuitService>().QuitApplication);
            playTypeButtons.transform.localScale = Vector3.zero;
            
            playButton.onClick.AddListener(() => { 
                LayoutElement layoutelement = playTypeButtons.GetComponent<LayoutElement>();
                Sequence sequence = DOTween.Sequence();
                sequence
                .Append(playButton.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InExpo))
                .AppendInterval(0.05f)
                .Join(playTypeButtons.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InExpo))
                .OnStart(() => { playTypeButtons.SetActive(true); })
                .OnComplete(() => { 
                    playButton.gameObject.SetActive(false);
                    layoutelement.ignoreLayout = false;
                })
                .Play();
            });

            localButton.onClick.AddListener(() => {
                PlayerAmountButtons.SetActive(true);
            });


            GameState gameState = Reflex.Core.Container.RootContainer.Resolve<GameState>();

            foreach (Button button in PlayerAmountButtons.GetComponentsInChildren<Button>(true))
            {
                TMP_Text textComponent = button.GetComponentInChildren<TMP_Text>();
                if (!textComponent)
                    Debug.LogError("No Text component found on button", button);

                int count = 0;
                if (!Int32.TryParse(textComponent.text, out count))
                    Debug.LogError("Parsed text of button was not a number");

                button.onClick.AddListener(() => { gameRoundManager.StartNewRound(count); gameState.Value = EGameState.InGame; });
            }
        }
    }
}
