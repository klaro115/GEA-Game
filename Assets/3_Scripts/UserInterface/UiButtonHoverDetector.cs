using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.UI
{
	/// <summary>
	/// Interface for receiving and reacting to mouse hover events on UI elements.
	/// </summary>
	public interface IUiMouseHoverListener
	{
		void notifyMouseEnter(PointerEventData eventData);
	}

	[RequireComponent(typeof(Selectable))]
	public class UiButtonHoverDetector : MonoBehaviour, IPointerEnterHandler
	{
		#region Fields

		private IUiMouseHoverListener listener = null;
		private Selectable selectable = null;

		#endregion
		#region Methods

		public void setListener(IUiMouseHoverListener inListener)
		{
			listener = inListener;
			selectable = GetComponent<Selectable>();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if(listener != null && selectable.interactable)
			{
				listener.notifyMouseEnter(eventData);
			}
		}

		#endregion
	}
}
