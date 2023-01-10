using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using System.Threading;

public class AwaitableDialog : MonoBehaviour
{
    [SerializeField] private RectTransform root;
    [SerializeField] private GameObject backgroundImage;
    [SerializeField] private GameObject dialogCloseButton;
    [SerializeField] private GameObject yesButton;
    [SerializeField] private GameObject noButton;

    private CancellationTokenSource tokenSource;

    private ObservableEventTrigger backgroundImageTrigger;
    private ObservableEventTrigger dialogCloseButtonTrigger;
    private ObservableEventTrigger yesButtonTrigger;
    private ObservableEventTrigger noButtonTrigger;

    public const string PREFAB_PATH = "Prefabs/AwaitableDialog";

    private void Start()
    {
        this.backgroundImageTrigger = this.backgroundImage.AddComponent<ObservableEventTrigger>();
        this.dialogCloseButtonTrigger = this.dialogCloseButton.AddComponent<ObservableEventTrigger>();
        this.yesButtonTrigger = this.yesButton.AddComponent<ObservableEventTrigger>();
        this.noButtonTrigger = this.noButton.AddComponent<ObservableEventTrigger>();

        this.dialogCloseButtonTrigger.OnPointerDownAsObservable().Subscribe(eventData => eventData.pointerEnter.gameObject.transform.localScale = Vector3.one * 0.9f).AddTo(this);
        this.yesButtonTrigger.OnPointerDownAsObservable().Subscribe(eventData => eventData.pointerEnter.gameObject.transform.localScale = Vector3.one * 0.9f).AddTo(this);
        this.noButtonTrigger.OnPointerDownAsObservable().Subscribe(eventData => eventData.pointerEnter.gameObject.transform.localScale = Vector3.one * 0.9f).AddTo(this);

        this.dialogCloseButtonTrigger.OnPointerUpAsObservable().Subscribe(eventData => eventData.pointerEnter.gameObject.transform.localScale = Vector3.one * 1.0f).AddTo(this);
        this.yesButtonTrigger.OnPointerUpAsObservable().Subscribe(eventData => eventData.pointerEnter.gameObject.transform.localScale = Vector3.one * 1.0f).AddTo(this);
        this.noButtonTrigger.OnPointerUpAsObservable().Subscribe(eventData => eventData.pointerEnter.gameObject.transform.localScale = Vector3.one * 1.0f).AddTo(this);
    }

    private async UniTask windowOpenTask()
    {
        while (true)
        {
            this.root.localScale = Vector3.Lerp(this.root.localScale, Vector3.one, 8.0f * Time.deltaTime);
            if (this.root.localScale.magnitude >= 1.00f)
            {
                this.root.localScale = Vector3.one;
                break;
            }

            await UniTask.Yield();
        }
    }

    private async UniTask windowCloseTask()
    {
        while (true)
        {
            this.root.localScale = Vector3.Lerp(this.root.localScale, Vector3.zero, 8.0f * Time.deltaTime);
            if (this.root.localScale.magnitude <= 0.3f)
            {
                this.root.localScale = Vector3.zero;
                Destroy(this.gameObject);
                break;
            }

            await UniTask.Yield();
        }
    }

    public async UniTask HideTask()
    {
        this.root.transform.localScale = Vector3.zero;
        await UniTask.DelayFrame(1);
    }

    public async UniTask<string> ShowTask()
    {
        this.windowOpenTask().Forget();
        var whichButtonClickTask = Observable.Merge(
            this.backgroundImageTrigger.OnPointerClickAsObservable(),
            this.dialogCloseButtonTrigger.OnPointerClickAsObservable(),
            this.yesButtonTrigger.OnPointerClickAsObservable(),
            this.noButtonTrigger.OnPointerClickAsObservable()
        ).FirstOrDefault().ToUniTask();

        var clickedButtonName = (await whichButtonClickTask).pointerClick.name;
        this.windowCloseTask().Forget();

        return clickedButtonName;
    }

    public static async UniTask<GameObject> LoadPrefabAsync()
    {
        var awaitableDialogGo = await Resources.LoadAsync(PREFAB_PATH) as GameObject;
        return awaitableDialogGo;
    }
}