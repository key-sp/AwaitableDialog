using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using System.Threading;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject awaitableDialogOpenButton;
    [SerializeField] private Text clickedButtonNameText;

    private CancellationTokenSource tokenSource;
    private ObservableEventTrigger awaitableDialogOpenButtonTrigger;

    private void Start()
    {
        this.awaitableDialogOpenButtonTrigger = this.awaitableDialogOpenButton.AddComponent<ObservableEventTrigger>();

        this.awaitableDialogOpenButtonTrigger.OnPointerDownAsObservable().Subscribe(eventData => eventData.pointerEnter.gameObject.transform.localScale = Vector3.one * 0.9f).AddTo(this);
        this.awaitableDialogOpenButtonTrigger.OnPointerUpAsObservable().Subscribe(eventData => eventData.pointerEnter.gameObject.transform.localScale = Vector3.one * 1.0f).AddTo(this);
        this.awaitableDialogOpenButtonTrigger.OnPointerClickAsObservable().Subscribe(_ => this.openAwaitableDialogTask().Forget()).AddTo(this);
    }

    private async UniTask openAwaitableDialogTask()
    {
        var awaitableDialogGo = UnityEngine.Object.Instantiate(await AwaitableDialog.LoadPrefabAsync());
        awaitableDialogGo.transform.SetParent(this.canvas.transform, false);

        var awaitableDialog = awaitableDialogGo.GetComponent<AwaitableDialog>();
        await awaitableDialog.HideTask();

        var clickedButtonName = await awaitableDialog.ShowTask();
        this.clickedButtonNameText.text = clickedButtonName;
    }
}