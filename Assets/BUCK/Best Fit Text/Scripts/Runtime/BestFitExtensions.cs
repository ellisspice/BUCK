#region LICENSE
/*
File modified by Ellis Spice, 2020
Original file created and released by PlayGen Ltd, 2018
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
#if TEXT_MESH_PRO_INCLUDED
using TMPro;
#endif

namespace BUCK.BestFitText
{
    public static class BestFitExtensions
    {
        /// <summary>
        /// Set all Text on this Component and the children of this Component to be the biggest visible font size.
        /// </summary>
        /// <param name="component">All Text on the Component's GameObject and children of the Component's GameObject will be set to the biggest visible font size.</param>
        /// <param name="includeInactive">Optional. Should inactive GameObjects and Text Components be resized and used in resizing calculations?</param>
        /// <param name="newStrings">Optional. List of strings which will be used in resizing calculations to ensure it is visible in all Text Components being resized.</param>
        /// <returns>Returns the font size the Text has been set to.</returns>  
        public static float BestFit<T>(this T component, bool includeInactive = true, List<string> newStrings = null) where T : Component
        {
            return component ? BestFit(component.gameObject, includeInactive, newStrings) : 0;
        }

        /// <summary>
        /// Set all Text in this list of Components and the children of the Components to be the biggest visible font size.
        /// </summary>
        /// <param name="components">All Text on the Components' GameObjects and children of the Components' GameObjects will be set to the biggest visible font size.</param>
        /// <param name="includeInactive">Optional. Should inactive GameObjects and Text Components be resized and used in resizing calculations?</param>
        /// <param name="newStrings">Optional. List of strings which will be used in resizing calculations to ensure it is visible in all Text Components being resized.</param>
        /// <returns>Returns the font size the Text has been set to.</returns>  
        public static float BestFit<T>(this List<T> components, bool includeInactive = true, List<string> newStrings = null) where T : Component
        {
            return BestFit(components?.ToArray(), includeInactive, newStrings);
        }

        /// <summary>
        /// Set all Text in this IEnumerable of Components and the children of the Components to be the biggest visible font size.
        /// </summary>
        /// <param name="components">All Text on the Components' GameObjects and children of the Components' GameObjects will be set to the biggest visible font size.</param>
        /// <param name="includeInactive">Optional. Should inactive GameObjects and Text Components be resized and used in resizing calculations?</param>
        /// <param name="newStrings">Optional. List of strings which will be used in resizing calculations to ensure it is visible in all Text Components being resized.</param>
        /// <returns>Returns the font size the Text has been set to.</returns>  
        public static float BestFit<T>(this IEnumerable<T> components, bool includeInactive = true, List<string> newStrings = null) where T : Component
        {
            return BestFit(components?.ToArray(), includeInactive, newStrings);
        }

        /// <summary>
        /// Set all Text in this array of Components and the children of the Components be the biggest visible font size.
        /// </summary>
        /// <param name="components">All Text on the Components' GameObjects and children of the Components' GameObjects will be set to the biggest visible font size.</param>
        /// <param name="includeInactive">Optional. Should inactive GameObjects and Text Components be resized and used in resizing calculations?</param>
        /// <param name="newStrings">Optional. List of strings which will be used in resizing calculations to ensure it is visible in all Text Components being resized.</param>
        /// <returns>Returns the font size the Text has been set to.</returns>  
        public static float BestFit<T>(this T[] components, bool includeInactive = true, List<string> newStrings = null) where T : Component
        {
            //Null objects are removed (as otherwise we will hit errors) and GameObject is selected for each provided Component.
            return BestFit(components?.Where(obj => obj != null).Select(obj => obj.gameObject), includeInactive, newStrings);
        }

        /// <summary>
        /// Set all Text on this GameObject and the children of this GameObject to be the biggest visible font size.
        /// </summary>
        /// <param name="gameObject">All Text on the GameObject and children of the GameObject will be set to the biggest visible font size.</param>
        /// <param name="includeInactive">Optional. Should inactive GameObjects and Text Components be resized and used in resizing calculations?</param>
        /// <param name="newStrings">Optional. List of strings which will be used in resizing calculations to ensure it is visible in all Text Components being resized.</param>
        /// <returns>Returns the font size the Text has been set to.</returns>  
        public static float BestFit(this GameObject gameObject, bool includeInactive = true, List<string> newStrings = null)
        {
            return BestFit(new[] { gameObject }, includeInactive, newStrings);
        }

        /// <summary>
        /// Set all Text in this list of GameObjects and the children of the GameObjects to be the biggest visible font size.
        /// </summary>
        /// <param name="gameObjects">All Text on the GameObjects and the children of the GameObjects will be set to the biggest visible font size.</param>
        /// <param name="includeInactive">Optional. Should inactive GameObjects and Text Components be resized and used in resizing calculations?</param>
        /// <param name="newStrings">Optional. List of strings which will be used in resizing calculations to ensure it is visible in all Text Components being resized.</param>
        /// <returns>Returns the font size the Text has been set to.</returns>  
        public static float BestFit(this List<GameObject> gameObjects, bool includeInactive = true, List<string> newStrings = null)
        {
            return BestFit(gameObjects?.ToArray(), includeInactive, newStrings);
        }

        /// <summary>
        /// Set all Text in this IEnumerable of GameObjects and the children of the GameObjects to be the biggest visible font size.
        /// </summary>
        /// <param name="gameObjects">All Text on the GameObjects and the children of the GameObjects will be set to the biggest visible font size.</param>
        /// <param name="includeInactive">Optional. Should inactive GameObjects and Text Components be resized and used in resizing calculations?</param>
        /// <param name="newStrings">Optional. List of strings which will be used in resizing calculations to ensure it is visible in all Text Components being resized.</param>
        /// <returns>Returns the font size the Text has been set to.</returns>  
        public static float BestFit(this IEnumerable<GameObject> gameObjects, bool includeInactive = true, List<string> newStrings = null)
        {
            return BestFit(gameObjects?.ToArray(), includeInactive, newStrings);
        }

        /// <summary>
        /// Set all Text in this Array of GameObjects and the children of the GameObjects to be the biggest visible font size.
        /// </summary>
        /// <param name="gameObjects">All Text on the GameObjects and the children of the GameObjects will be set to the biggest visible font size.</param>
        /// <param name="includeInactive">Optional. Should inactive GameObjects and Text Components be resized and used in resizing calculations?</param>
        /// <param name="newStrings">Optional. List of strings which will be used in resizing calculations to ensure it is visible in all Text Components being resized.</param>
        /// <returns>Returns the font size the Text has been set to.</returns>  
        public static float BestFit(this GameObject[] gameObjects, bool includeInactive = true, List<string> newStrings = null)
        {
            //Remove null and duplicate GameObjects.
            gameObjects = gameObjects?.Where(go => go != null).Distinct().ToArray();
            var previousSmallSize = 0f;
            var smallestFontSize = 0f;
            var checkCount = 0;
            //Remove inactive GameObjects if includeInactive has been set to false.
            if (!includeInactive)
            {
                gameObjects = gameObjects?.Where(go => go.activeInHierarchy).ToArray();
            }
            //Return zero if there's no GameObjects to perform best fit on.
            if (gameObjects == null || gameObjects.Length == 0)
            {
                return 0;
            }
            //Calculations are run up to ten times to ensure that an accurate and consistent value is used.
            while ((Mathf.Approximately(previousSmallSize, 0) || Mathf.Approximately(previousSmallSize, smallestFontSize)) && checkCount < 10)
            {
                previousSmallSize = smallestFontSize;
                //Trigger LayoutGroups to rebuild so that text areas are more accurate.
                var layoutGroups = gameObjects.SelectMany(g => g.GetComponentsInParent<LayoutGroup>(includeInactive).Select(l => (RectTransform)l.transform));
                layoutGroups = layoutGroups.Concat(gameObjects.Select(g => (RectTransform)g.transform));
                layoutGroups = layoutGroups.Concat(gameObjects.SelectMany(g => g.GetComponentsInChildren<LayoutGroup>(includeInactive).Select(l => (RectTransform)l.transform))).Distinct().ToList();
                layoutGroups.ToList().Reverse();
                foreach (var lg in layoutGroups)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(lg);
                }

                //Trigger AspectRatioFitters to resize so that text areas are more accurate.
                var aspectRatioFitters = gameObjects.SelectMany(g => g.GetComponentsInParent<AspectRatioFitter>(includeInactive));
                aspectRatioFitters = aspectRatioFitters.Concat(gameObjects.Select(g => g.GetComponent<AspectRatioFitter>()).Where(a => a != null));
                aspectRatioFitters = aspectRatioFitters.Concat(gameObjects.SelectMany(g => g.GetComponentsInChildren<AspectRatioFitter>(includeInactive))).Distinct().ToList();
                aspectRatioFitters.ToList().Reverse();
                foreach (var arf in aspectRatioFitters)
                {
                    var arfRectTransform = (RectTransform)arf.transform;
                    //Same logic that AspectRatioFitter uses when resizing.
                    switch (arf.aspectMode)
                    {
                        case AspectRatioFitter.AspectMode.HeightControlsWidth:
                            arfRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, arfRectTransform.rect.height * arf.aspectRatio);
                            break;
                        case AspectRatioFitter.AspectMode.WidthControlsHeight:
                            arfRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, arfRectTransform.rect.width / arf.aspectRatio);
                            break;
                        case AspectRatioFitter.AspectMode.None:
                            break;
                        case AspectRatioFitter.AspectMode.FitInParent:
                        case AspectRatioFitter.AspectMode.EnvelopeParent:
                            arfRectTransform.anchorMin = Vector2.zero;
                            arfRectTransform.anchorMax = Vector2.one;
                            arfRectTransform.anchoredPosition = Vector2.zero;
                            var sizeDelta = Vector2.zero;
                            var parentSize = Vector2.zero;
                            var parent = arfRectTransform.parent as RectTransform;
                            if (parent)
                            {
                                parentSize = parent.rect.size;
                            }
                            if ((parentSize.y * arf.aspectRatio < parentSize.x) ^ (arf.aspectMode == AspectRatioFitter.AspectMode.FitInParent))
                            {
                                sizeDelta.y = (parentSize.x / arf.aspectRatio) - parentSize[1] * (arfRectTransform.anchorMax[1] - arfRectTransform.anchorMin[1]);
                            }
                            else
                            {
                                sizeDelta.y = (parentSize.y * arf.aspectRatio) - parentSize[0] * (arfRectTransform.anchorMax[0] - arfRectTransform.anchorMin[0]);
                            }
                            arfRectTransform.sizeDelta = sizeDelta;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                foreach (var go in gameObjects)
                {
#if TEXT_MESH_PRO_INCLUDED
                    //Dropdown templates need to be set to active in order to ensure their text is properly sized.
                    var tmpdropObj = go.GetComponentsInChildren<TMP_Dropdown>(includeInactive);
                    foreach (var drop in tmpdropObj)
                    {
                        drop.template.gameObject.SetActive(true);
                    }
#endif
                    //Dropdown templates need to be set to active in order to ensure their text is properly sized.
                    var dropObj = go.GetComponentsInChildren<Dropdown>(includeInactive);
                    foreach (var drop in dropObj)
                    {
                        drop.template.gameObject.SetActive(true);
                    }
#if TEXT_MESH_PRO_INCLUDED
                    var tmptextObj = go.GetComponentsInChildren<TextMeshProUGUI>(includeInactive);
                    foreach (var text in tmptextObj)
                    {
                        //Add dropdown options to newStrings list to ensure all possible dropdown options will fit.
                        var dropdown = text.GetComponentInParent<TMP_Dropdown>();
                        var newStringList = newStrings == null ? new List<string>() : new List<string>(newStrings);
                        if (dropdown)
                        {
                            newStringList.AddRange(dropdown.options.Select(o => o.text));
                        }
                        //Get the best fit size for this text.
                        var newSize = GetBestFitSize(text, newStringList);
                        //If newSize is smaller than the current smallestFontSize or if smallestFontSize has not been set, set smallestFontSize to equal newSize.
                        if (!Mathf.Approximately(newSize, 0f) && (newSize < smallestFontSize || Mathf.Approximately(smallestFontSize, 0f)))
                        {
                            smallestFontSize = newSize;
                        }
                    }
#endif
                    var textObj = go.GetComponentsInChildren<Text>(includeInactive);
                    foreach (var text in textObj)
                    {
                        //Add dropdown options to newString list to ensure all possible dropdown options will fit.
                        var dropdown = text.GetComponentInParent<Dropdown>();
                        var newStringList = newStrings == null ? new List<string>() : new List<string>(newStrings);
                        if (dropdown)
                        {
                            newStringList.AddRange(dropdown.options.Select(o => o.text));
                        }
                        //Get the best fit size for this text.
                        var newSize = GetBestFitSize(text, newStringList);
                        //If newSize is smaller than the current smallestFontSize or if smallestFontSize has not been set, set smallestFontSize to equal newSize.
                        if (!Mathf.Approximately(newSize, 0f) && (newSize < smallestFontSize || Mathf.Approximately(smallestFontSize, 0f)))
                        {
                            smallestFontSize = newSize;
                        }
                    }
                }
                //Set the text to the smallestFontSize gathered.
                foreach (var go in gameObjects)
                {
#if TEXT_MESH_PRO_INCLUDED
                    var tmptextObj = go.GetComponentsInChildren<TextMeshProUGUI>(includeInactive);
                    foreach (var text in tmptextObj)
                    {
                        text.fontSize = smallestFontSize;
                        var bestFitBehaviour = text.GetComponentsInParent<BestFit>(true);
                        //If there is a parent BestFit Component and the font size is out of the bounds of the defined min and max, adjust value accordingly.
                        if (bestFitBehaviour.Length > 0 && bestFitBehaviour[0].MinFontSize > text.fontSize)
                        {
                            text.fontSize = Mathf.Clamp(text.fontSize, bestFitBehaviour[0].MinFontSize, bestFitBehaviour[0].MaxFontSize);
                        }
                    }
#endif
                    var textObj = go.GetComponentsInChildren<Text>(includeInactive);
                    foreach (var text in textObj)
                    {
                        text.fontSize = (int)smallestFontSize;
                        var bestFitBehaviour = text.GetComponentsInParent<BestFit>(true);
                        //If there is a parent BestFit Component and the font size is out of the bounds of the defined min and max, adjust value accordingly.
                        if (bestFitBehaviour.Length > 0 && bestFitBehaviour[0].MinFontSize > text.fontSize)
                        {
                            text.fontSize = Mathf.Clamp(text.fontSize, bestFitBehaviour[0].MinFontSize, bestFitBehaviour[0].MaxFontSize);
                        }
                    }
                    
#if TEXT_MESH_PRO_INCLUDED
                    //Disable all previously enabled dropdown templates.
                    var tmpdropObj = go.GetComponentsInChildren<TMP_Dropdown>(includeInactive);
                    foreach (var drop in tmpdropObj)
                    {
                        drop.template.gameObject.SetActive(false);
                    }
#endif
                    //Disable all previously enabled dropdown templates.
                    var dropObj = go.GetComponentsInChildren<Dropdown>(includeInactive);
                    foreach (var drop in dropObj)
                    {
                        drop.template.gameObject.SetActive(false);
                    }
                }
                checkCount++;
            }
            return smallestFontSize;
        }

        /// <summary>
        /// Get the best fit size for this Text Component.
        /// </summary>
        /// <param name="text">The Text Component to get the best fit size of.</param>
        /// <param name="newStrings">List of strings which will be used in resizing calculations to ensure it is visible in this Text Component.</param>
        /// <returns>Returns the font size the Text has been set to.</returns>  
        private static float GetBestFitSize(Text text, List<string> newStrings)
        {
            var smallestFontSize = 0;
            var currentText = text.text;
            if (newStrings == null)
            {
                newStrings = new List<string> { currentText };
            }
            if (!newStrings.Contains(currentText))
            {
                newStrings.Add(currentText);
            }
            var bestFitBehaviour = text.GetComponentInParent<BestFit>();
            foreach (var s in newStrings)
            {
                text.text = s;
                text.resizeTextForBestFit = true;
                text.resizeTextMinSize = bestFitBehaviour ? bestFitBehaviour.MinFontSize : 1;
                //Plus one as Unity's Text Best Fit always max out at one below value provided.
                text.resizeTextMaxSize = (bestFitBehaviour ? bestFitBehaviour.MaxFontSize : 300) + 1;
                //Set to max possible to start all text at an equal point.
                text.fontSize = text.resizeTextMaxSize;
                //Invalidate and populate to fully redraw text.
                text.cachedTextGenerator.Invalidate();
                text.cachedTextGenerator.Populate(text.text, text.GetGenerationSettings(text.rectTransform.rect.size));
                //Disable BestFit on Component.
                text.resizeTextForBestFit = false;
                var cachedTextGenerator = text.cachedTextGenerator;
                var newSize = cachedTextGenerator.fontSizeUsedForBestFit;
                //NewSizeRescale is used to scale down newSize if rect used for text is bigger than rect for the GameObject.
                var newSizeRescale = text.rectTransform.rect.size.x / cachedTextGenerator.rectExtents.size.x;
                if (text.rectTransform.rect.size.y / text.cachedTextGenerator.rectExtents.size.y < newSizeRescale)
                {
                    newSizeRescale = text.rectTransform.rect.size.y / text.cachedTextGenerator.rectExtents.size.y;
                }
                newSizeRescale = RescaleFontSize(text, newSizeRescale);
                //Multiply newSize by newSizeScale and round down to the nearest int.
                newSize = Mathf.FloorToInt(newSize * newSizeRescale);
                //If newSize is smaller than the current smallestFontSize or if smallestFontSize has not been set, set smallestFontSize to equal newSize.
                if (newSize < smallestFontSize || smallestFontSize == 0)
                {
                    smallestFontSize = newSize;
                }
            }
            //Reset the text to display what it was previously.
            text.text = currentText;
            return Mathf.Min(smallestFontSize, bestFitBehaviour ? bestFitBehaviour.MaxFontSize : 300);
        }

#if TEXT_MESH_PRO_INCLUDED
        /// <summary>
        /// Get the best fit size for this TMP_Text Component.
        /// </summary>
        /// <param name="text">The TMP_Text Component to get the best fit size of.</param>
        /// <param name="newStrings">List of strings which will be used in resizing calculations to ensure it is visible in this Text Component.</param>
        /// <returns>Returns the font size the Text has been set to.</returns>  
        private static float GetBestFitSize(TMP_Text text, List<string> newStrings)
        {
            var smallestFontSize = 0f;
            var currentText = text.text;
            if (newStrings == null)
            {
                newStrings = new List<string> { currentText };
            }
            if (!newStrings.Contains(currentText))
            {
                newStrings.Add(currentText);
            }
            var bestFitBehaviour = text.GetComponentInParent<BestFit>();
            foreach (var s in newStrings)
            {
                text.text = s;
                text.enableAutoSizing = true;
                text.fontSizeMin = bestFitBehaviour ? bestFitBehaviour.MinFontSize : 1f;
                text.fontSizeMax = bestFitBehaviour ? bestFitBehaviour.MaxFontSize : 1000f;
                //Set to max possible to start all text at an equal point.
                text.fontSize = text.fontSizeMax;
                //Force the text mesh to be updated.
                text.Rebuild(CanvasUpdate.PreRender);
                //Disable BestFit on Component.
                text.enableAutoSizing = false;
                var newSize = text.fontSize;
                //NewSizeRescale is used to scale down newSize if rect used for text is bigger than rect for the GameObject.
                var newSizeRescale = RescaleFontSize(text, 1f);
                //Multiply newSize by newSizeScale and round down to the nearest int.
                newSize *= newSizeRescale;
                //If newSize is smaller than the current smallestFontSize or if smallestFontSize has not been set, set smallestFontSize to equal newSize.
                if (newSize < smallestFontSize || Mathf.Approximately(smallestFontSize, 0))
                {
                    smallestFontSize = newSize;
                }
            }
            //Reset the text to display what it was previously.
            text.text = currentText;
            return Mathf.Min(smallestFontSize, bestFitBehaviour ? bestFitBehaviour.MaxFontSize : 300);
        }
#endif
        
        /// <summary>
        /// Calculate how much the font size needs to be rescaled based on current canvas settings
        /// </summary>
        /// <param name="text">The Component that BestFit is being performed on.</param>
        /// <param name="rescale">Base amount for rescaling.</param>
        /// <returns>Returns the amount the gathered best fit size should be scaled by.</returns>  
        private static float RescaleFontSize(Component text, float rescale)
        {
            //Get highest CanvasScaler and Canvas relative to this Text Component.
            var canvasScaler = text.GetComponentInParent<CanvasScaler>();
            while (canvasScaler != null && canvasScaler.transform.parent != null && canvasScaler.transform.parent.GetComponentInParent<CanvasScaler>())
            {
                canvasScaler = canvasScaler.transform.parent.GetComponentInParent<CanvasScaler>();
            }
            var canvas = text.GetComponentInParent<Canvas>();
            while (canvas != null && canvas.transform.parent != null && canvas.transform.parent.GetComponentInParent<Canvas>())
            {
                canvas = canvas.transform.parent.GetComponentInParent<Canvas>();
            }
            //Log logic copied from https://bitbucket.org/Unity-Technologies/ui/src/a3f89d5f7d145e4b6fa11cf9f2de768fea2c500f/UnityEngine.UI/UI/Core/Layout/CanvasScaler.cs?at=2017.3&fileviewer=file-view-default
            //Allows calculation to be accurate from the first frame.
            if (canvas && canvasScaler && canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                var referenceResolution = canvasScaler.referenceResolution;
                var worldCamera = canvas.worldCamera;
                var worldCameraRectSize = worldCamera ? worldCamera.rect.size : Vector2.zero;
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || (canvas.renderMode == RenderMode.ScreenSpaceCamera && !worldCamera) || (canvas.renderMode == RenderMode.ScreenSpaceCamera && worldCamera && worldCamera.orthographic))
                {
                    float scalerFactor;
                    switch (canvasScaler.screenMatchMode)
                    {
                        case CanvasScaler.ScreenMatchMode.MatchWidthOrHeight:
                        {
                            var logWidth = Mathf.Log(Screen.width / referenceResolution.x, 2);
                            var logHeight = Mathf.Log(Screen.height / referenceResolution.y, 2);
                            var logWeightedAverage = Mathf.Lerp(logWidth, logHeight, canvasScaler.matchWidthOrHeight);
                            scalerFactor = Mathf.Pow(2, logWeightedAverage);
                            break;
                        }
                        case CanvasScaler.ScreenMatchMode.Expand:
                            scalerFactor = Mathf.Min(Screen.width / referenceResolution.x, Screen.height / referenceResolution.y);
                            break;
                        case CanvasScaler.ScreenMatchMode.Shrink:
                            scalerFactor = Mathf.Max(Screen.width / referenceResolution.x, Screen.height / referenceResolution.y);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    if (canvas.renderMode == RenderMode.ScreenSpaceCamera && worldCamera && worldCamera.orthographic)
                    {
                        scalerFactor *= worldCamera.orthographicSize / (Screen.height * 0.5f * worldCameraRectSize.y);
                    }
                    //Adjust rescale based on difference between Canvas LocalScale and scalerFactor.
                    rescale *= canvas.transform.localScale.x / scalerFactor;
                    if (canvas.renderMode == RenderMode.ScreenSpaceCamera && worldCamera)
                    {
                        //Rescale newSizeRescale based on difference between the actual resolution and the resolution the canvas is using.
                        var actualRes = (float)Screen.width / Screen.height;
                        var scalerRes = referenceResolution.x / referenceResolution.y;
                        var resDiff = actualRes / scalerRes;
                        //Additional rescale based on camera rect size and magnitude.
                        var scaleUp = (1 / Mathf.Max(worldCameraRectSize.x, worldCameraRectSize.y)) * worldCameraRectSize.magnitude;
                        //Smallest value based on normalized x and y should be used, but value can not be less than 1.
                        rescale *= Mathf.Max(1, Mathf.Min(worldCameraRectSize.normalized.x * scaleUp * Mathf.Max(1, resDiff), worldCameraRectSize.normalized.y * scaleUp * (1 / Mathf.Min(1, resDiff))));
                    }
                }
            }
            return rescale;
        }
    }
}