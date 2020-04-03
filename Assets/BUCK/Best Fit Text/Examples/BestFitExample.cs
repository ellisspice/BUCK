using BUCK.BestFitText;
using UnityEngine;

namespace BUCK.Examples
{
    public class BestFitExample : MonoBehaviour
    {
        [SerializeField]
        private GameObject _anchorObject;
        [SerializeField]
        private GameObject _layoutObject;
        [SerializeField]
        private GameObject _layoutAndAspectObject;

        private void Start()
        {
            BestFitText();
        }

        private void OnEnable()
        {
            BestFit.ResolutionChange += BestFitText;
            BestFitText();
        }

        private void OnDisable()
        {
            BestFit.ResolutionChange += BestFitText;
        }

        private void BestFitText()
        {
            _anchorObject.BestFit();
            _layoutObject.BestFit();
            _layoutAndAspectObject.BestFit();
        }
    }
}