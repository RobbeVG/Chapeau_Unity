using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Seacore.UI
{

    public class UIPlayerMenu : MonoBehaviour
    {
        [SerializeField]
        GameObject playerBarPrefab;

        [SerializeField]
        Button addPLayerButton;

        [SerializeField]
        GameObject contentPlayerView;

        LinkedList<UIPlayerBar> uiPlayerBarsLList = new LinkedList<UIPlayerBar>();

        public int AmountPlayers { get { return uiPlayerBarsLList.Count; } }

        private void OnValidate()
        {
            if (contentPlayerView == null)
            {
                contentPlayerView = transform.Find("Content").gameObject;
            }
        }

        private void Awake()
        {
            addPLayerButton.onClick.AddListener(AddPlayerBar);
            UIPlayerBar[] playerBars = GetComponentsInChildren<UIPlayerBar>();

            foreach (UIPlayerBar playerBarItem in playerBars)
            {
                uiPlayerBarsLList.AddLast(playerBarItem);
                playerBarItem.GetComponent<UIPlayerBar>().AddListenerButton(DeletePlayerBar);
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

            uiPlayerBar.PlayerName += ' ' + AmountPlayers;

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
