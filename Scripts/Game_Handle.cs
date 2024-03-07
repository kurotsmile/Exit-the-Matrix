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

    [Header("Panel Game")]
    public GameObject panel_menu;
    public GameObject panel_play;
    public GameObject panel_done;

    [Header("UI Game")]
    public Text txt_play_timer;
    public Text txt_play_fruit;

    [Header("Ui Victory")]
    public Text txt_victory_timer;
    public Text txt_victory_scores;

    [Header("Effect")]
    public GameObject[] obj_effect;

    public AudioSource[] sound;

    private int fruit = 0;
    private float timer=0f;
    private bool is_play = false;

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
    }

    private void check_exit_app()
    {

    }

    public void btn_play_now()
    {
        this.carrot.play_sound_click();
        this.is_play = true;
        this.panel_menu.SetActive(false);
        this.panel_play.SetActive(true);
        this.panel_done.SetActive(false);
        this.player_play.start_play();
    }

    public void btn_back_menu()
    {
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
        int milliseconds = (int)(1000 * (time - minutes * 60 - seconds));
        return string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, milliseconds);
    }

    public void btn_show_setting()
    {
        Carrot.Carrot_Box box_setting = this.carrot.Create_Setting();
        box_setting.set_act_before_closing(this.act_before_close);
    }

    private void act_before_close()
    {

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
        this.is_play = false;
        this.panel_play.SetActive(false);
        this.panel_done.SetActive(true);
        this.txt_victory_timer.text = this.FormatTime(this.timer);
        this.txt_victory_scores.text = this.fruit.ToString();
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
}
