using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.XR.WSA.WebCam;
using System.Collections.Generic;

public class webCamtexture : MonoBehaviour
{

    PhotoCapture photoCaptureObject = null;
    Texture2D targetTexture = null;
    public float h, s, v;
    // Use this for initialization
    public void Start()
    {
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        // Create a PhotoCapture object
        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject) {
            photoCaptureObject = captureObject;
            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 0.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            // Activate the camera
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result) {
                // Take a picture
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });


    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {
            Debug.Log("Success. Changing things now!");
            List<byte> imageBufferList = new List<byte>();

            // Copy the raw IMFMediaBuffer data into our empty byte list.
            photoCaptureFrame.CopyRawImageDataIntoBuffer(imageBufferList);

            // In this example, we captured the image using the BGRA32 format.
            // So our stride will be 4 since we have a byte for each rgba channel.
            // The raw image data will also be flipped so we access our pixel data
            // in the reverse order.
            int stride = 4;
            float denominator = 1.0f / 255.0f;
            float r = 0; float g = 0; float b = 0;
            int counter = 0;
            for (int i = imageBufferList.Count - 1; i >= 0; i -= stride)
            {
                counter++;
                //				float a = (int)(imageBufferList[i - 0]) * denominator;
                r += (int)(imageBufferList[i - 1]) * denominator;
                g += (int)(imageBufferList[i - 2]) * denominator;
                b += (int)(imageBufferList[i - 3]) * denominator;


            }
            Debug.Log("Color Array"+ r.ToString());

            // Now we could do something with the array such as texture.SetPixels() or run image processing on the list

            r /= counter;
            g /= counter;
            b /= counter;

            Color hsvColor, rgbColor;
            rgbColor = new Color(r, g, b, 1);
            
            Color.RGBToHSV(rgbColor, out h, out s, out v);
            Color32[] pix = targetTexture.GetPixels32();
            for (int i = 0; i < pix.Length; i++)
            {
                pix[i].r = (byte)(r*255);
                pix[i].g = (byte)(g*255);
                pix[i].b = (byte)(b*255);

            }
            Debug.Log("Current HSV = (" + h.ToString() + ", " + s.ToString() + ", " + v.ToString() + ")");
            //targetTexture.SetPixels32(pix);
            //targetTexture.Apply();
            //GetComponent<MeshRenderer>().material.mainTexture = targetTexture;

        }
        else
        {
            Debug.Log("Failed!! and Fucked ");
        }
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }
    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown our photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }
}