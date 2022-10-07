using System.Net.Security;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using UnityEngine.UI;

public class VedioMagr : BaseManager<VedioMagr>
{
    //唯一的全屏播放器
    private VideoPlayer cVedio = null;
    //唯一视频背景布
    private GameObject VedioBk;
    //渐入渐出Animation
    private Animation animation;
     
    private bool isPlayVedio=false;
    //播放完回调函数
    private UnityAction stopfun;
    public VedioMagr()
    {
        MonoMgr.GetInstance().AddUpdateListener(Update);
    }

    private void Update()
    {
        if(cVedio!=null&&cVedio.isPlaying&&VedioBk.GetComponent<RawImage>()!=null){
              VedioBk.GetComponent<RawImage>().texture=cVedio.texture;
              isPlayVedio=true;
        }
        if(isPlayVedio&&!cVedio.isPlaying){
            isPlayVedio=false;
            StopVedio();
        }
    }

    /// <summary>
    /// 播放视频
    /// </summary>
    /// <param name="name"></param>
    /// <param name="bkname"></param>
    /// <param name="fun">回调函数，一些具体控制的禁用</param>
    public void PlaycVedio(string name,UnityAction beginfun,UnityAction stopfun)
    {
        if(VedioBk==null)
        {
            GameObject bk = ResMgr.GetInstance().Load<GameObject>("UI/VideoBK");
            VedioBk=bk;
        }
        if(VedioBk.GetComponent<Animation>()==null)
        {
            animation=VedioBk.AddComponent<Animation>();
            AnimationClip inclip=ResMgr.GetInstance().Load<AnimationClip>("Video/VideoIn");
            AnimationClip outclip=ResMgr.GetInstance().Load<AnimationClip>("Video/VideoOut");
            animation.AddClip(inclip,"VideoIn");
            animation.AddClip(outclip,"VideoOut");
        }else{
            animation= VedioBk.GetComponent<Animation>();
            
        }
        if(VedioBk.GetComponent<VideoPlayer>()==null)
        {
            cVedio=VedioBk.AddComponent<VideoPlayer>();
        }
        else
        cVedio=VedioBk.GetComponent<VideoPlayer>();
        if(VedioBk.GetComponent<RawImage>()==null){
            VedioBk.AddComponent<RawImage>();
        }
        //设置父物体
        VedioBk.transform.SetParent(UIManager.GetInstance().canvas.Find("Top").transform,false);
        //异步加载视频 加载完成后 播放
        ResMgr.GetInstance().LoadAsync<VideoClip>("Video/" + name, (clip) =>
        {
            cVedio.clip = clip;
            cVedio.playOnAwake = false;
            cVedio.playbackSpeed = 1;
            VedioBk.GetComponent<CanvasRenderer>().cullTransparentMesh=false;
            beginfun?.Invoke();
            this.stopfun=stopfun;
            animation.Play("VideoIn");
            cVedio.Play();
        });

    }
    /// <summary>
    /// 停止播放视频
    /// </summary>
    /// <param name="fun">回调</param>
    public void StopVedio()
    {
        if (cVedio == null)
            return;
        cVedio.Stop();
        VedioBk.GetComponent<RawImage>().texture=null;
        VedioBk.GetComponent<CanvasRenderer>().cullTransparentMesh=true;
        animation.Play("VideoOut");
        stopfun?.Invoke();
    }
     

}
