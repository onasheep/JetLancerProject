using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DangerPanel : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Sprite> sprites;
    private int spriteIdx;
    private Image damage_warning;
    private Image damage_panel;
    private float spriteTime = 0.5f;
    private float spriteStay = 1f;
    void Start()
    {
        Init();
    }
    private void Init()
    {
        spriteIdx = 0;
        damage_warning = this.gameObject.FindChildObj("damage_warning").GetComponent<Image>();
        damage_panel = this.GetComponent<Image>();
    }

    public IEnumerator SwapImage()
    {
        spriteIdx = 0;
        damage_warning.color = new Color(1f, 1f, 1f, 1f);
        damage_panel.color = new Color(1f, 0f, 0f, 100f / 255f);

        while (spriteIdx < sprites.Count)
        {
            damage_warning.sprite = sprites[spriteIdx];
            yield return new WaitForSeconds(spriteTime / sprites.Count);
            spriteIdx += 1;

        }
        yield return new WaitForSeconds(spriteStay);
        damage_panel.color = new Color(1f, 1f, 1f, 0f);
        damage_warning.color = new Color(1f, 1f, 1f, 0f);
    }


}
