using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class OverlayController : MonoBehaviour
{
    public int stage;

    private Text _tx;
    private Animator _am;

    private static readonly int Exit = Animator.StringToHash("Exit");

    private readonly string[] _prefixes = new[]
    {
        "hallowed ",
        "infinite ",
        "harmonic ",
        "crimson ",
        "delirium ",
        "frenzy ",
        "sorrow ",
        "skeletal ",
        "torture ",
        "plague ",
        "bora",
        "osea",
        "yuk",
        "peath",
        "dri",
        "drua",
        "bru",
        "kren"
    };

    private readonly string[] _suffixes = new[]
    {
        " realm",
        " fields",
        " ground",
        " land",
        " kingdom",
        " domain",
        " pasture",
        "mas",
        "iru",
        "agdell",
        "sura",
        "'xin",
        "siux"
    };

    private void Awake()
    {
        _tx = GetComponentInChildren<Text>();
        _am = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(PlayOverlay());
    }

    private string GenerateName() =>
        $"{_prefixes[Random.Range(0, _prefixes.Length)]}{_suffixes[Random.Range(0, _suffixes.Length)]}\n{Stats.Instance.Realm}"
            .Replace("  ", " ");

    private IEnumerator PlayOverlay()
    {
        _tx.text = GenerateName();

        yield return new WaitForSeconds(3.0f);

        HideOverlay();
    }

    private void HideOverlay()
    {
        _am.SetTrigger(Exit);
    }
}