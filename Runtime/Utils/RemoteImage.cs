using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace VervePlace.YandexGames
{
  public class RemoteImage
  {
    private readonly string _url;
    
    private Texture2D _texture;
    public Texture2D Texture
    {
      get
      {
        if (!IsDownloadFinished)
          throw new InvalidOperationException($"Attempt to get {nameof(Texture)} while {nameof(IsDownloadFinished)} = {IsDownloadFinished}");

        if (!IsDownloadSuccessful)
          throw new InvalidOperationException($"Attempt to get {nameof(Texture)} while {nameof(IsDownloadSuccessful)} = {IsDownloadSuccessful}");

        return _texture;
      }
    }

    public bool IsDownloadFinished { get; private set; }
    public bool IsDownloadSuccessful { get; private set; }
    public string DownloadErrorMessage { get; private set; }

    public RemoteImage(string url) => 
      _url = url;

    public IEnumerator Download(Action<Texture2D> successCallback = null, Action<string> errorCallback = null)
    {
      using (UnityWebRequest downloadTextureWebRequest = UnityWebRequestTexture.GetTexture(_url))
      {
        UnityWebRequestAsyncOperation downloadOperation = downloadTextureWebRequest.SendWebRequest();

        while (!downloadOperation.isDone)
          yield return null;

        IsDownloadFinished = true;

        if (downloadOperation.webRequest.result != UnityWebRequest.Result.Success)
        {
          DownloadErrorMessage = downloadOperation.webRequest.error;
        }
        else
        {
          _texture = DownloadHandlerTexture.GetContent(downloadTextureWebRequest);

          if (_texture is not null)
            IsDownloadSuccessful = true;
          else
            DownloadErrorMessage = "Getting content of a downloaded texture has failed.";
        }
      }

      if (IsDownloadSuccessful)
        successCallback?.Invoke(_texture);
      else
        errorCallback?.Invoke(DownloadErrorMessage);
    }
  }
}