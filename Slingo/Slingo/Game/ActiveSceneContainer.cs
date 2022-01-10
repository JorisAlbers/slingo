using System;
using System.Reactive;
using System.Reactive.Linq;
using OBSWebsocketDotNet;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Slingo.Game
{
    public class ActiveSceneContainer : ReactiveObject
    {
        public ActiveSceneContainer(OBSWebsocket obsWebsocket)
        {
            IObservable<EventPattern<string>> sceneChanged = Observable.FromEventPattern<string>(obsWebsocket, "SceneChanged");
            sceneChanged.ObserveOn(RxApp.MainThreadScheduler).Subscribe(x => SetScene(x.EventArgs));
            // set initial scene
            SetScene(obsWebsocket.GetCurrentScene().Name);
        }

        [Reactive] public string Scene { get; private set; }

        [Reactive] public bool IsTeam1Active { get; private set; }
        [Reactive] public bool IsTeam2Active { get; private set; }


        public void SetScene(string sceneName)
        {
            Scene = sceneName;

            if (sceneName == "studio")
            {
                IsTeam1Active = true;
                IsTeam2Active = true;
                return;
            }

            IsTeam1Active = sceneName.Contains("team 1");
            IsTeam2Active = sceneName.Contains("team 2");
        }
    }
}