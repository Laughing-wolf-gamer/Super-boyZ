using UnityEngine;
using System.Collections;
namespace GamerWolf.Utils{
	[ExecuteInEditMode]
	public class CameraAnchor : MonoBehaviour {
		public enum AnchorType {
			BottomLeft,
			BottomCenter,
			BottomRight,
			MiddleLeft,
			MiddleCenter,
			MiddleRight,
			TopLeft,
			TopCenter,
			TopRight,
		};
		[SerializeField] private AnchorType anchorType;
		[SerializeField] private Vector3 anchorOffset;

		private IEnumerator updateAnchorRoutine; 

		private void Start () {
			updateAnchorRoutine = UpdateAnchorAsync();
			StartCoroutine(updateAnchorRoutine);
		}

		
		private IEnumerator UpdateAnchorAsync() {
			uint cameraWaitCycles = 0;
			while(ViewportHandler.i == null) {
				++cameraWaitCycles;
				yield return new WaitForEndOfFrame();
			}
			if (cameraWaitCycles > 0) {
				print(string.Format("CameraAnchor found ViewportHandler instance after waiting {0} frame(s). You might want to check that ViewportHandler has an earlie execution order.", cameraWaitCycles));
			}
			UpdateAnchor();
			updateAnchorRoutine = null;
		}

		private void UpdateAnchor() {
			switch(anchorType) {
			case AnchorType.BottomLeft:
				SetAnchor(ViewportHandler.i.BottomLeft);
				break;
			case AnchorType.BottomCenter:
				SetAnchor(ViewportHandler.i.BottomCenter);
				break;
			case AnchorType.BottomRight:
				SetAnchor(ViewportHandler.i.BottomRight);
				break;
			case AnchorType.MiddleLeft:
				SetAnchor(ViewportHandler.i.MiddleLeft);
				break;
			case AnchorType.MiddleCenter:
				SetAnchor(ViewportHandler.i.MiddleCenter);
				break;
			case AnchorType.MiddleRight:
				SetAnchor(ViewportHandler.i.MiddleRight);
				break;
			case AnchorType.TopLeft:
				SetAnchor(ViewportHandler.i.TopLeft);
				break;
			case AnchorType.TopCenter:
				SetAnchor(ViewportHandler.i.TopCenter);
				break;
			case AnchorType.TopRight:
				SetAnchor(ViewportHandler.i.TopRight);
				break;
			}
		}

		private void SetAnchor(Vector3 anchor) {
			Vector3 newPos = anchor + anchorOffset;
			if (!transform.position.Equals(newPos)) {
				transform.position = newPos;
			}
		}

		
		private void Update () {
			if (updateAnchorRoutine == null) {
				updateAnchorRoutine = UpdateAnchorAsync();
				StartCoroutine(updateAnchorRoutine);
			}
		}
		
	}

}
