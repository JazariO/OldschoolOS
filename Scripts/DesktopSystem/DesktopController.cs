using Proselyte.Sigils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FMODUnity;
using UnityEngine.InputSystem;

namespace Proselyte.OldschoolOS
{
    public class DesktopController : MonoBehaviour
    {
        [SerializeField] PlayerSaveDataSO playerSaveDataSO;
        [SerializeField] GameObject media_explorer_prefab_file_image;
        [SerializeField] Transform media_explorer_panel_content_transform;
        [SerializeField] RectTransform desktop_cursor_transform;
        [SerializeField] Sprite audioFileImage;

        [SerializeField] GameEvent OnPhotoTaken;
        [SerializeField] GameEvent OnInspectDisengageBegin;
        [SerializeField] GameEvent OnMediaExplorerFileSelect;

        [SerializeField] MediaExplorerDetailsPanelDataSO mediaExplorerDetailsPanelDataSO;
        [SerializeField] RenderTexture photographic_camera_viewport_rendertexture;

        [SerializeField] FMODUnity.EventReference fmod_event_desktop_click_single;

        [SerializeField] Transform desktopAudioTransform;

        [Header("Details Panel References")]
        [SerializeField] Image details_panel_display_image;
        [SerializeField] TMP_Text details_panel_timestamp_TMP;
        // TODO(Jazz): also store audio for playback 

        private int photos_stored_count;
        private bool desktop_in_use;

        private void OnEnable()
        {
            OnPhotoTaken?.RegisterListener(HandlePhotoTaken);
            OnInspectDisengageBegin?.RegisterListener(HandleInspectDisengage);
            OnMediaExplorerFileSelect?.RegisterListener(HandleFileSelected);
        }
        private void OnDisable()
        {
            OnPhotoTaken?.UnregisterListener(HandlePhotoTaken);
            OnInspectDisengageBegin?.UnregisterListener(HandleInspectDisengage);
            OnMediaExplorerFileSelect?.UnregisterListener(HandleFileSelected);
        }

        private void Start()
        {
            desktop_in_use = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }

        private void Update()
        {
            if(desktop_in_use)
            {
                // update cursor position
                desktop_cursor_transform.localPosition =
                    new Vector2(Mathf.Floor(playerSaveDataSO.desktop_canvas_mouse_position.x / 4) * 4, Mathf.Floor(playerSaveDataSO.desktop_canvas_mouse_position.y / 4) * 4);

                // play click audio
                if(Mouse.current.leftButton.wasPressedThisFrame)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(fmod_event_desktop_click_single, desktopAudioTransform.position);
                }
            }
        }

        public void HandlePhotoTaken()
        {
            // TODO(Jazz): add upload sound emanating from desktop when receiving photo data from photographic camera

            // create new image prefab in the media explorer program
            GameObject file_image_gameObject = Instantiate(media_explorer_prefab_file_image, media_explorer_panel_content_transform);

            CrunchOSFile crunch_OS_file = file_image_gameObject.GetComponent<CrunchOSFile>();
            if(crunch_OS_file == null) Debug.LogError("prefab missing crunch os file component for photo.");

            // set file type to image
            crunch_OS_file.crunchFileData.isImage = true;

            // set timestamp
            crunch_OS_file.crunchFileData.timeStamp = "Timestamp: 24/03/1999";

            // add listeners to eventrigger to handle highlighting photos and adding them to the details panel
            EventTrigger eventTrigger = file_image_gameObject.GetComponent<EventTrigger>();
            EventTrigger.Entry entryPointerEnter = new EventTrigger.Entry() { eventID = EventTriggerType.PointerEnter };
            entryPointerEnter.callback.AddListener((data) =>
            {
                crunch_OS_file.crunchFileData.fileSelectionImage.color = new Color(crunch_OS_file.crunchFileData.fileSelectionImage.color.r, crunch_OS_file.crunchFileData.fileSelectionImage.color.g, crunch_OS_file.crunchFileData.fileSelectionImage.color.b, 1f);
            });
            EventTrigger.Entry entryPointer = new EventTrigger.Entry() { eventID = EventTriggerType.PointerEnter };
            entryPointerEnter.callback.AddListener((data) =>
            {
                crunch_OS_file.crunchFileData.fileSelectionImage.color = new Color(crunch_OS_file.crunchFileData.fileSelectionImage.color.r, crunch_OS_file.crunchFileData.fileSelectionImage.color.g, crunch_OS_file.crunchFileData.fileSelectionImage.color.b, 1f);
            });
            eventTrigger.triggers.Add(entryPointerEnter);

            // convert byte data to photos for the images
            int width = photographic_camera_viewport_rendertexture.width;
            int height = photographic_camera_viewport_rendertexture.height;
            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture.LoadImage(playerSaveDataSO.photo_taken_bytes);

            // assign converted photo sprite to display image
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
            crunch_OS_file.crunchFileData.fileDisplayImage.sprite = sprite;
        }

        public void HandleInspectEngage()
        {
            desktop_in_use = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void HandleInspectDisengage()
        {
            desktop_in_use = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void HandleFileSelected()
        {
            details_panel_display_image.sprite = mediaExplorerDetailsPanelDataSO.crunch_OS_file_data.fileDisplayImage.sprite;
            details_panel_timestamp_TMP.text = mediaExplorerDetailsPanelDataSO.crunch_OS_file_data.timeStamp;

            if(!mediaExplorerDetailsPanelDataSO.crunch_OS_file_data.isImage)
            {
                //TODO(Jazz): handle showing the audio playback section in the details panel for audio files
            }
        }
    }
}
