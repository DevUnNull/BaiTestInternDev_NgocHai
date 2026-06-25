using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    public InteractionScanner scanner;
    public Button interactionButton;
    public Text interactionText;

    private void Start()
    {
        if (interactionButton != null)
        {
            interactionButton.gameObject.SetActive(false);
            interactionButton.onClick.AddListener(OnButtonClicked);
        }

        if (scanner != null)
        {
            scanner.OnInteractableInRange += ShowButton;
            scanner.OnInteractableOutOfRange += HideButton;
        }
    }

    private void OnDestroy()
    {
        if (scanner != null)
        {
            scanner.OnInteractableInRange -= ShowButton;
            scanner.OnInteractableOutOfRange -= HideButton;
        }
    }

    private void ShowButton(IInteractable interactable)
    {
        if (interactionButton != null)
        {
            interactionButton.gameObject.SetActive(true);
            if (interactionText != null)
            {
                interactionText.text = interactable.GetInteractionPrompt();
            }
        }
    }

    private void HideButton()
    {
        if (interactionButton != null)
        {
            interactionButton.gameObject.SetActive(false);
        }
    }

    private void OnButtonClicked()
    {
        if (scanner != null)
        {
            scanner.InteractWithCurrentObject();
        }
    }
}
