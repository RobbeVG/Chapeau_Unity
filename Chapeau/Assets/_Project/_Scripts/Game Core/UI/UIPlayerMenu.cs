using Reflex.Attributes;
using Reflex.Extensions;
using Seacore.Common;
using Seacore.Game;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Seacore.UI
{

    public class UIPlayerMenu : MonoBehaviour
    {
        [Inject]
        WindowManager windowManager;

        [SerializeField]
        GameObject playerBarPrefab;

        [SerializeField]
        Button addPLayerButton;

        [SerializeField]
        GameObject contentPlayerView;

        [SerializeField]
        Button playGameButton;

        LinkedList<UIPlayerBar> uiPlayerBarsLList = new LinkedList<UIPlayerBar>();

        public int AmountPlayers { get { return uiPlayerBarsLList.Count; } }


        private void Awake()
        {
            addPLayerButton.onClick.AddListener(AddPlayerBar);

            GameRoundManager grm = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetSceneContainer().Single<GameRoundManager>();
            GameState gameState = Reflex.Core.Container.RootContainer.Resolve<GameState>();
            Window thisWindow = GetComponent<Window>();

            playGameButton.onClick.AddListener(() =>
            {
                windowManager.CloseWindow(thisWindow);

                LinkedList<Player> players = new LinkedList<Player>();
                foreach (UIPlayerBar item in uiPlayerBarsLList)
                {
                    players.AddLast(new Player(item.PlayerName));
                }

                grm.StartNewRound(players); 
                gameState.Value = EGameState.InGame;                 
            });
        }

        private void Start()
        {
            UIPlayerBar[] playerBars = GetComponentsInChildren<UIPlayerBar>();

            foreach (UIPlayerBar playerBarItem in playerBars)
            {
                uiPlayerBarsLList.AddLast(playerBarItem);
                playerBarItem.GetComponent<UIPlayerBar>().AddListenerButton(DeletePlayerBar);
                playerBarItem.PlayerName += ' ' + uiPlayerBarsLList.Count.ToString();
            }
        }

        private void OnDestroy()
        {
            addPLayerButton.onClick.RemoveAllListeners();
        }


        private void AddPlayerBar()
        {
            GameObject playerBar = Instantiate(playerBarPrefab, contentPlayerView.transform);
            UIPlayerBar uiPlayerBar = playerBar.GetComponent<UIPlayerBar>();
            uiPlayerBar.AddListenerButton(DeletePlayerBar);
            uiPlayerBarsLList.AddLast(uiPlayerBar);

            uiPlayerBar.PlayerName += ' ' + AmountPlayers.ToString();

            if (AmountPlayers > 2)
            {
                foreach (UIPlayerBar item in uiPlayerBarsLList)
                {
                    item.ButtonComponent.interactable = true;
                }
            }
        }

        private void DeletePlayerBar(UIPlayerBar playerBar)
        {
            uiPlayerBarsLList.Remove(playerBar);
            Destroy(playerBar.gameObject);

            //Check new count
            if (AmountPlayers <= 2)
            {
                foreach (UIPlayerBar item in uiPlayerBarsLList)
                {
                    item.ButtonComponent.interactable = false;
                }
            }
        }
    }
}
