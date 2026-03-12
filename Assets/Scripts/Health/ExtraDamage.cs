using UnityEngine;

public class ExtraDamage : MonoBehaviour
{
    [SerializeField] private Vector2 extraKnockedPower;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!collision.CompareTag("Player")) return;
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.KnockedPower = extraKnockedPower;
            player.KnowcBack(transform.position.x);
        }
        //if (!collision.CompareTag("PlayerDamage_1")) return;
        if (collision.CompareTag("PlayerDamage_1"))
        {
            PlayerController _player = collision.GetComponentInParent<PlayerController>();
            _player.KnockedPower = extraKnockedPower;
            _player.KnowcBack(transform.position.x);
        }
        
      /*  var playerr = collision.GetComponent<PlayerController>() ;
        if (collision.CompareTag("Player"))
        {
            player.KnockedPower = extraKnockedPower;
            player.KnowcBack(transform.position.x);
        }*/
     
        

        
    }
}
