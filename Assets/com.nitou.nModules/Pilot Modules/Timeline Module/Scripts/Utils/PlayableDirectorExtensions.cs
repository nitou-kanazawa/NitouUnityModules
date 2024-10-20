using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// [参考]
//  qiita: PlayableDirectorの再生をawaitする拡張メソッド https://qiita.com/rarudonet/items/f7fc7453ec1c6126af38
//  qiita: PlayableDirectorのイベントをObservable化する https://qiita.com/Teach/items/a4d669aabcad19011b07
//  ゆいブロ: TimelineをScriptから扱うときのTips https://www.yui-tech-blog.com/entry/timeline-script-tips/


namespace nitou.Timeline{

    public static class PlayableDirectorExtensions{

        /// <summary>
        /// 終了を待機できる再生メソッド
        /// </summary>
        public static UniTask PlayAsync(this PlayableDirector self) {
            self.Play();

            // ポーズor終了まで待機
            return UniTask.WaitWhile(() => self.state == PlayState.Playing);
        }




        public static IObservable<PlayableDirector> PlayedAsObservable(this PlayableDirector self) {
            return Observable.FromEvent<PlayableDirector>(
                h => self.played += h,
                h => self.played -= h);
        }

        public static IObservable<PlayableDirector> PausedAsObservable(this PlayableDirector self) {
            return Observable.FromEvent<PlayableDirector>(
                h => self.paused += h,
                h => self.paused -= h);
        }

        public static IObservable<PlayableDirector> StoppedAsObservable(this PlayableDirector self) {
            return Observable.FromEvent<PlayableDirector>(
                h => self.stopped += h,
                h => self.stopped -= h);
        }

        // [メモ]
        // Stoppedは"WrapMode"がNoneであれば,「再生終了時」または「GameObjectごとDestroyした時」に呼ばれる．
        // しかし，コンポーネントがenabled==falseになったときには呼ばれない（中断扱い）．







    }
}
