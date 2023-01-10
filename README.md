# AwaitableDialog


This component is a simple dialog component that can wait using await to get the result of the selected button.
Then click one of the buttons below. You should see results.

![AwaitableDialog](https://user-images.githubusercontent.com/33142993/211649895-8333fc82-b2e2-4cff-b012-eed2aad42750.gif)

```C#
var clickedButtonName = await awaitableDialog.ShowTask();
this.clickedButtonNameText.text = clickedButtonName;
```
You can wait for the dialog result very simply like the code above.


Depends on UniRx, UniTask and assets below.
2D Casual UI HD | 2D Icons | Unity Asset Store
https://assetstore.unity.com/packages/2d/gui/icons/2d-casual-ui-hd-82080#reviews

Import this asset if you want to use this dialog component like a gif image.
