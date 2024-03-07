using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Handle : MonoBehaviour
{
    [Header("Obj Game")]
    public Carrot.Carrot carrot;
    public Player_main player_play;
    public Image img_status_audio;
    public Sprite sp_icon_audio_on;
    public Sprite sp_icon_audio_off;
    public GameObject[] obj_map;

    [Header("Panel Game")]
    public GameObject panel_menu;
    public GameObject panel_play;
    public GameObject panel_done;
    public GameObject panel_high_scores;

    [Header("UI Game")]
    public Text txt_play_timer;
    public Text txt_play_fruit;
    public Text txt_menu_high_scores;

    [Header("Ui Victory")]
    public Text txt_victory_timer;
    public Text txt_victory_scores;
    public Text txt_victory_high_scores;
    public Text txt_victory_completions;

    [Header("Effect")]
    public GameObject[] obj_effect;

    public AudioSource[] sound;

    private int high_scores = 0;
    private int victory_completions = 0;
    private int fruit = 0;
    private float timer=0f;
    private bool is_play = false;
    private int index_map = -1;
    private Carrot.Carrot_Window_Msg msg_question_reset;

    void Start()
    {
        this.carrot.Load_Carrot(this.check_exit_app);
        this.panel_menu.SetActive(true);
        this.panel_play.SetActive(false);
        this.panel_done.SetActive(false);

        this.player_play.load();
        this.player_play.ready_play();
        if(this.carrot.get_status_sound()) this.carrot.game.load_bk_music(this.sound[0]);
        this.check_status_audio();
        this.load_map();

        this.high_scores = PlayerPrefs.GetInt("high_scores", 0);
        if (this.high_scores > 0)
            this.panel_high_scores.SetActive(true);
        else
            this.panel_high_scores.SetActive(false);

        this.victory_completions = PlayerPrefs.GetInt("victory_completions", 0);
        this.txt_menu_high_scores.text = this.high_scores.ToString();
    }

    private void load_map()
    {
        for (int i = 0; i < this.obj_map.Length; i++) this.obj_map[i].SetActive(false);

        int rand_sel_map = this.get_random_map();
        this.index_map = rand_sel_map;
        this.obj_map[rand_sel_map].SetActive(true);
    }

    private int get_random_map()
    {
        int index_r;
        int rand_sel_map = Random.Range(0, this.obj_map.Length);
        if (this.index_map == -1)
            index_r = rand_sel_map;
        else
        {
            if(this.index_map== rand_sel_map)
                index_r = get_random_map();
            else
                index_r = rand_sel_map;
        }
        return index_r;
    }

    private void check_exit_app()
    {
        if (this.panel_play.activeInHierarchy)
        {
            this.btn_back_menu();
            this.carrot.set_no_check_exit_app();
        }else if (this.panel_done.activeInHierarchy)
        {
            this.btn_back_menu();
            this.carrot.set_no_check_exit_app();
        }
    }

    public void btn_play_now()
    {
        this.timer = 0f;
        this.fruit = 0;
        this.carrot.ads.show_ads_Interstitial();
        this.load_map();
        this.carrot.play_sound_click();
        this.is_play = true;
        this.panel_menu.SetActive(false);
        this.panel_play.SetActive(true);
        this.panel_done.SetActive(false);
        this.player_play.start_play();
        this.update_info_ui();
    }

    public void btn_back_menu()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.play_sound_click();
        this.is_play = false;
        this.panel_play.SetActive(false);
        this.panel_menu.SetActive(true);
        this.panel_done.SetActive(false);
        this.player_play.ready_play();
    }

    void Update()
    {
        if (this.is_play)
        {
            this.timer += 1f * Time.deltaTime;
            this.txt_play_timer.text = this.FormatTime(this.timer).ToString();
        }
    }

    public string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        int milliseconds = (int)(100 * (time - minutes * 60 - seconds));
        return string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, milliseconds);
    }

    public void btn_show_setting()
    {
        this.carrot.ads.show_ads_Interstitial();
        Carrot.Carrot_Box box_setting = this.carrot.Create_Setting();
        box_setting.set_act_before_closing(this.act_before_close);
    }

    private void act_before_close(List<string> list_change)
    {
        foreach(string s in list_change)
        {
            if (s == "list_bk_music") this.carrot.game.load_bk_music(this.sound[0]);
        }
    }

    public void btn_show_user_login()
    {
        this.carrot.user.show_login();
    }

    public void btn_show_rate()
    {
        this.carrot.show_rate();
    }

    public void btn_show_share()
    {
        this.carrot.show_share();
    }

    public void btn_show_rank_player()
    {
        this.carrot.game.Show_List_Top_player();
    }

    public void play_sound(int index_sound)
    {
        if (this.carrot.get_status_sound()) this.sound[index_sound].Play();
    }

    public void btn_change_status_audio()
    {
        this.carrot.Change_status_sound(null);
        if (this.carrot.get_status_sound())
        {
           
            this.sound[0].Pause();
        }  
        else
        {
            this.sound[0].Play();
            this.carrot.play_sound_click();
        }
            
        this.check_status_audio();
    }

    private void check_status_audio()
    {
        if (this.carrot.get_status_sound())
            this.img_status_audio.sprite = this.sp_icon_audio_on;
        else
            this.img_status_audio.sprite = this.sp_icon_audio_off;
    }

    public void show_victory()
    {
        this.victory_completions++;
        PlayerPrefs.SetInt("victory_completions", this.victory_completions);
        if (this.fruit > this.high_scores)
        {
            this.high_scores = this.fruit;
            PlayerPrefs.SetInt("high_scores", this.high_scores);
            this.txt_menu_high_scores.text = this.high_scores.ToString();
            this.carrot.game.update_scores_player(this.high_scores);
            this.panel_high_scores.SetActive(true);
        }

        this.carrot.ads.show_ads_Interstitial();
        this.is_play = false;
        this.panel_play.SetActive(false);
        this.panel_done.SetActive(true);
        this.txt_victory_timer.text = this.FormatTime(this.timer);
        this.txt_victory_scores.text = this.fruit.ToString();
        this.txt_victory_high_scores.text = this.high_scores.ToString();
        this.txt_victory_completions.text = this.victory_completions.ToString();
        this.play_sound(2);
        this.player_play.GetComponent<PlayerMoveController>().enabled = false;
    }

    public void add_fruit()
    {
        this.fruit++;
        this.txt_play_fruit.text = this.fruit.ToString();
        this.play_sound(3);
    }

    public void create_effect(Vector3 pos,int index_effect=0)
    {
        GameObject effect_obj = Instantiate(this.obj_effect[index_effect]);
        effect_obj.transform.position = pos;
        effect_obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        Destroy(effect_obj, 2f);
    }

    public void btn_reset()
    {
       this.msg_question_reset=this.carrot.show_msg("Reset game","Are you sure you want to reset the level?", act_reset_yes, act_reset_no);
    }

    private void act_reset_yes()
    {
        this.play_sound(1);
        this.btn_play_now();
        this.msg_question_reset.close();
    }

    private void act_reset_no()
    {
        this.carrot.play_sound_click();
        this.msg_question_reset.close();
    }

    private void update_info_ui()
    {
        this.txt_play_timer.text = this.FormatTime(this.timer);
        this.txt_play_fruit.text = this.fruit.ToString();
    }
}
