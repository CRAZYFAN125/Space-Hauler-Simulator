using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Crazy.SHS.Core.Menu
{
    public class MainMenuScript : MonoBehaviour
    {
        /// <summary>
        /// Canvas where everything will be, default will search for "MenuCanvas"
        /// </summary>
        [SerializeField]
        private GameObject canvas;
        /// <summary>
        /// Font used for Text Elements
        /// </summary>
        public Font font;

        /// <summary>
        /// Does loading go async to game? Use SetAsync() on button to change that
        /// </summary>
        bool Async = false;


        private void Start()
        {
            if (canvas == null)
                canvas = GameObject.Find("MenuCanvas");

            if (canvas == null)
            {
                Debug.LogWarning("Couldn't find canvas named \"MenuCanvas\", and no object assigned, abording...");
                Destroy(this); return;
            }
        }

        public void SetAsync(bool async)
        {
            Async = async;
        }

        public void LoadScene(string name)
        {
            if (name == SceneManager.GetActiveScene().name)
            {
                print("Cannot go to the same scene!");
                return;
            }
            if (Async)
            {
                BuildSceneLoader(out Slider slider, out Text text);
                AsyncOperation operation = SceneManager.LoadSceneAsync(name);
                while (!operation.isDone)
                {
                    slider.value = operation.progress * 100;
                    text.text = $"{operation.progress * 100}%";
                }
            }
            else
            {
                SceneManager.LoadScene(name);
            }
        }

        public void LoadScene(int id)
        {
            if (id == SceneManager.GetActiveScene().buildIndex)
            {
                print("Cannot go to the same scene!");
                return;
            }
            if (Async)
            {
                BuildSceneLoader(out Slider slider, out Text text);
                AsyncOperation operation = SceneManager.LoadSceneAsync(id);
                while (!operation.isDone)
                {
                    slider.value = operation.progress * 100;
                    text.text = $"{operation.progress * 100}%";
                }
            }
            else
            {
                SceneManager.LoadScene(id);
            }
        }

        void BuildSceneLoader(out Slider _loadingBar, out Text _loadingText)
        {
            #region Setting up canvas
            var canvasRect = canvas.GetComponent<RectTransform>().rect;

            //SceneManager.LoadSceneAsync(id);

            GameObject panel = new("LoadingPanel");
            panel.transform.SetParent(canvas.transform, false);
            panel.AddComponent<Image>().color = Color.black;
            panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, canvasRect.width);
            panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, canvasRect.height);


            GameObject loadingBar = new("LoadingBar");
            loadingBar.transform.SetParent(panel.transform, false);
            loadingBar.AddComponent<Image>().color = Color.blue;
            loadingBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(/*canvasRect.width / 2*/ 0, /*-canvasRect.height/2*/0);
            loadingBar.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            loadingBar.GetComponent<RectTransform>().anchorMax = new Vector2(canvasRect.width + (canvasRect.width * 0.2f), canvasRect.height + (canvasRect.height * 0.2f));


            panel.AddComponent<Slider>().fillRect = loadingBar.GetComponent<RectTransform>();
            var sliderComponent = panel.GetComponent<Slider>();
            sliderComponent.direction = Slider.Direction.BottomToTop;
            sliderComponent.maxValue = 100;

            GameObject loadingText = new("LoadingText");
            loadingText.transform.SetParent(panel.transform, false);
            var loadingTextComponent = loadingText.AddComponent<Text>();
            loadingTextComponent.font = font;
            loadingTextComponent.alignment = TextAnchor.MiddleCenter;
            loadingTextComponent.text = "Yo!";
            loadingText.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            loadingText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, canvasRect.width * .45f);
            loadingText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, canvasRect.height * .45f);

            loadingTextComponent.resizeTextForBestFit = true;
            loadingTextComponent.fontSize = 300;

            loadingText.AddComponent<Outline>().effectDistance = new Vector2(2, 2);
            loadingText.GetComponent<Outline>().effectColor = Color.black;
            #endregion

            _loadingBar = sliderComponent;
            _loadingText = loadingTextComponent;
        }

        public void OpenScoreboard()
        {
            //TODO
        }
    }
}