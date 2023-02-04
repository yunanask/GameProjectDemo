using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip attack0;
    public static AudioClip attack1;
    public static AudioClip attack2;
    public static AudioClip button;
    public static AudioClip character0; 
    public static AudioClip character1;
    public static AudioClip fanying0;
    public static AudioClip fanying1;
    public static AudioClip fanying2;
    public static AudioClip move0;
    public static AudioClip move1;
    public static AudioClip move2;
    public static AudioClip move3;
    public static AudioClip skill0;
    public static AudioClip skill1;
    public static AudioClip skill2;
    public static AudioClip game0;
    public static AudioClip game1;
    public static AudioClip game2;
    public static AudioClip yuansu0;
    public static AudioClip yuansu1;
    public static AudioClip yuansu2;
    public static AudioClip yuansu3;
    public static AudioClip yuansu4;
    public static AudioClip yuansu5;

    public static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        attack0 = Resources.Load<AudioClip>("jianattack");
        attack1 = Resources.Load<AudioClip>("bowattack");
        attack2 = Resources.Load<AudioClip>("paoattack");

        button = Resources.Load<AudioClip>("click");

        character0 = Resources.Load<AudioClip>("die");
        character1 = Resources.Load<AudioClip>("select");

        fanying0 = Resources.Load<AudioClip>("kezhi");
        fanying1 = Resources.Load<AudioClip>("explore");
        fanying2 = Resources.Load<AudioClip>("dian");

        game0 = Resources.Load<AudioClip>("turn");
        game1 = Resources.Load<AudioClip>("win");
        game2 = Resources.Load<AudioClip>("defeat");

        move0 = Resources.Load<AudioClip>("moveslow");
        move1 = Resources.Load<AudioClip>("movequick");
        move2 = Resources.Load<AudioClip>("movepaoche");
        move3 = Resources.Load<AudioClip>("baoxiang");

        skill0 = Resources.Load<AudioClip>("jianskill");
        skill1 = Resources.Load<AudioClip>("bowskill");
        skill2 = Resources.Load<AudioClip>("paoskill");

        yuansu0 = Resources.Load<AudioClip>("water");
        yuansu1 = Resources.Load<AudioClip>("fire");
        yuansu2 = Resources.Load<AudioClip>("grass");
        yuansu3 = Resources.Load<AudioClip>("thunder1");
        yuansu4 = Resources.Load<AudioClip>("thunder2");
        yuansu5 = Resources.Load<AudioClip>("thunder3");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void PlayAttack(int type)
    {
        if (type == 0) audioSrc.PlayOneShot(attack0);
        if (type == 1) audioSrc.PlayOneShot(attack1);
        if (type == 2) audioSrc.PlayOneShot(attack2);
    }

    public static void Playbutton()
    {
        audioSrc.PlayOneShot(button);
    }
    public static void Playcharacter(int type)
    {
        if (type == 0) audioSrc.PlayOneShot(character0);
        if (type == 1) audioSrc.PlayOneShot(character1);
    }
    public static void Playfanying(int type)
    {
        if (type == 0) audioSrc.PlayOneShot(fanying0);
        if (type == 1) audioSrc.PlayOneShot(fanying1);
        if (type == 2) audioSrc.PlayOneShot(fanying2);
    }
    public static void Playgame(int type)
    {
        if (type == 0) audioSrc.PlayOneShot(game0);
        if (type == 1) audioSrc.PlayOneShot(game1);
        if (type == 2) audioSrc.PlayOneShot(game2);
    }
    public static void Playmove(int type)
    {

        if (type == 0) audioSrc.PlayOneShot(move0,10f);
        if (type == 1) audioSrc.PlayOneShot(move1,10f);
        if (type == 2) audioSrc.PlayOneShot(move2,10f);
        if (type == 3) audioSrc.PlayOneShot(move3,10f);

    }
    public static void Playskill(int type)
    {
        if (type == 0) audioSrc.PlayOneShot(skill0);
        if (type == 1) audioSrc.PlayOneShot(skill1);
        if (type == 2) audioSrc.PlayOneShot(skill2);
    }
    public static void Playyuansu(int type)
    {
        if (type == 0) audioSrc.PlayOneShot(yuansu0);
        if (type == 1) audioSrc.PlayOneShot(yuansu1);
        if (type == 2) audioSrc.PlayOneShot(yuansu2);
        if (type == 3) audioSrc.PlayOneShot(yuansu3);
        if (type == 4) audioSrc.PlayOneShot(yuansu4);
        if (type == 5) audioSrc.PlayOneShot(yuansu5);
    }

}
