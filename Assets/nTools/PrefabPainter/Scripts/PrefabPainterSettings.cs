
#if UNITY_EDITOR


using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;


namespace nTools.PrefabPainter
{
    public enum OrientationMode
    {
        None,
        AlongSurfaceNormal,
        AlongBrushStroke,
        X,
        Y,
        Z,
    }

    public enum TransformMode
    {
        Relative,
        Absolute,
    }

    public enum Placement
    {
        World,
        HitObject,
        CustomObject,
    }

    public enum ScaleMode
    {
        Uniform,
        PerAxis,
    }
    
    [System.Serializable]
    public class BrushPreset
    {        
        public List<GameObject> prefabs = new List<GameObject>();

        public string name;

        public float brushSize;
        public float eraseBrushSize;
        public float brushSpacing;

        public Vector3 positionOffset;

        public TransformMode    orientationTransformMode;
        public OrientationMode  orientationMode;
        public bool             flipOrientation;
        public Vector3          rotation;
        public Vector3          randomizeOrientation;

        public TransformMode    scaleTransformMode;
        public ScaleMode        scaleMode;
        public float            scaleUniformMin;
        public float            scaleUniformMax;
        public Vector3          scalePerAxisMin;
        public Vector3          scalePerAxisMax;

		public bool 			selected = false;


        public GameObject prefab {
            get {
                return prefabs.Count > 0 ? prefabs[0] : null;
            }
        }


		public Texture2D prefabPreview {
			get {                
				if (prefab != null)
				{                    
					Texture2D previewTexture = AssetPreview.GetAssetPreview(prefab);
					if (previewTexture != null)
						return previewTexture;

					previewTexture = AssetPreview.GetMiniThumbnail(prefab);
					if (previewTexture != null)
						return previewTexture;

					previewTexture = (Texture2D)AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(prefab));
					if (previewTexture != null)
						return previewTexture;

     				return AssetPreview.GetMiniTypeThumbnail(typeof(GameObject));
				}
				return null;
			}
		}



        public BrushPreset() { Reset(); }
        public BrushPreset(GameObject newPrefab) { Reset(); AssignPrefab(newPrefab); name = newPrefab.name; }

        public BrushPreset(BrushPreset other)
        {
            Reset();

            prefabs = new List<GameObject>(other.prefabs);

            name = other.name;

            brushSize = other.brushSize;
            eraseBrushSize = other.eraseBrushSize;
            brushSpacing = other.brushSpacing;

            positionOffset = other.positionOffset;

            orientationTransformMode = other.orientationTransformMode;
            orientationMode = other.orientationMode;
            flipOrientation = other.flipOrientation;
            rotation = other.rotation;
            randomizeOrientation = other.randomizeOrientation;

            scaleTransformMode = other.scaleTransformMode;
            scaleMode = other.scaleMode;
            scaleUniformMin = other.scaleUniformMin;
            scaleUniformMax = other.scaleUniformMax;
            scalePerAxisMin = other.scalePerAxisMin;
            scalePerAxisMax = other.scalePerAxisMax;
        }


        public void Reset()
        {
            brushSize = 1.0f;
            eraseBrushSize = 1.0f;
            brushSpacing = 0.5f;

            positionOffset = new Vector3(0 ,0 ,0);

            orientationTransformMode = TransformMode.Relative;
            orientationMode = OrientationMode.AlongSurfaceNormal;
            flipOrientation = false;
            rotation = new Vector3(0, 0, 0);
            randomizeOrientation = new Vector3(0, 0, 0);


            scaleTransformMode = TransformMode.Relative;
            scaleMode = ScaleMode.Uniform;
            scaleUniformMin = 1.0f;
            scaleUniformMax = 1.0f;
            scalePerAxisMin = new Vector3(1, 1, 1);
            scalePerAxisMax = new Vector3(1, 1, 1);
        }

        public void AssignPrefab(GameObject newPrefab)
        {
            prefabs.Clear();
            prefabs.Add(newPrefab);
        }


    }





    //
    // class PrefabPainterSettings
    //
    public class PrefabPainterSettings : ScriptableObject
    {
        public bool paintOnSelected = false;
        public int  paintLayers = ~0;

        public bool overwritePrefabLayer = false;
        public int  prefabPlaceLayer = 0;

        public bool groupPrefabs = true;

        public float brushSizeMax = 20.0f;
        public float brushSpacingMax = 5.0f;


        public List<BrushPreset> presets = new List<BrushPreset>();


        public bool brushSettingsFoldout = true;
        public bool positionSettingsFoldout = true;
        public bool orientationSettingsFoldout = true;
        public bool scaleSettingsFoldout = true;
        public bool commonSettingsFoldout = true;


        void OnEnable()
        {
        }




		public bool HasMultipleSelectedPresets()
		{
			int selectedCount = 0;
			for (int i = 0; i < presets.Count; i++)
			{
				if (presets[i].selected)
					selectedCount++;

				if (selectedCount > 1)
					return true;
			}
			return false;
		}

		public bool HasSelectedPresets()
		{
			for (int i = 0; i < presets.Count; i++)
			{
				if (presets[i].selected)
					return true;				
			}
			return false;
		}

		public BrushPreset GetFirstSelectedPreset()
		{
			for (int i = 0; i < presets.Count; i++)
			{
				if (presets[i].selected)
					return presets[i];
			}
			return null;
		}

		public bool IsPresetSelected(int presetIndex)
		{
			if (presetIndex >= 0 && presetIndex < presets.Count)
			{
				return presets[presetIndex].selected;
			}
			return false;
		}


		public void SelectPreset(int presetIndex)
		{
			if (presetIndex >= 0 && presetIndex < presets.Count)
			{
				presets.ForEach ((preset) => preset.selected = false);
				presets[presetIndex].selected = true;
			}
		}

		public void SelectPresetAdd(int presetIndex)
		{
			if (presetIndex >= 0 && presetIndex < presets.Count)
			{
				presets[presetIndex].selected = true;
			}
		}

		public void SelectPresetRange(int toPresetIndex)
		{
			if (toPresetIndex < 0 && toPresetIndex >= presets.Count)
				return;

			int rangeMin = toPresetIndex;
			int rangeMax = toPresetIndex;

			for (int i = 0; i < presets.Count; i++)
			{
				if (presets[i].selected)
				{
					rangeMin = Mathf.Min(rangeMin, i);
					rangeMax = Mathf.Max(rangeMax, i);
				}
			}
			for (int i = rangeMin; i <= rangeMax; i++) {
				presets[i].selected = true;
			}
		}

		public void DeselectAllPresets()
		{
			presets.ForEach ((preset) => preset.selected = false);
		}


		public void DuplicateSelectedPresets()
		{
			if (!HasSelectedPresets ())
				return;

			Undo.RegisterCompleteObjectUndo(this, "Duplicate Preset(s)");

			for (int presetIndex = 0; presetIndex < presets.Count; presetIndex++)
			{
				if (presets[presetIndex].selected)
				{
					BrushPreset duplicate = new BrushPreset (presets [presetIndex]);

					presets [presetIndex].selected = false;
					duplicate.selected = true;
					
					presets.Insert(presetIndex, duplicate);

					presetIndex++; // move over new inserted duplicate
				}
			}
		}

		public void DeleteSelectedPresets()
		{
			if (!HasSelectedPresets ())
				return;

			Undo.RegisterCompleteObjectUndo (this, "Delete Preset(s)");

			presets.RemoveAll ((preset) => preset.selected);
		}

		public void ResetSelectedPresets()
		{
			if (!HasSelectedPresets ())
				return;

			Undo.RegisterCompleteObjectUndo (this, "Reset Preset(s)");

			presets.ForEach ((preset) => preset.Reset());
		}
    }

}

#endif
