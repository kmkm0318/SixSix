using DG.Tweening;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    #region 쉐이더 변수 이름
    private const string BACKGROUND_COLOR = "_BackgroundColor";
    private const string CIRCLE_COLOR = "_CircleColor";
    #endregion

    [Header("Background Settings")]
    [SerializeField] private Renderer _background;
    [SerializeField] private float _duration = 1f;

    #region 쉐이더 변수 ID
    private int _backgroundColorPropertyID;
    private int _circleColorPropertyID;
    #endregion

    private void Awake()
    {
        //머티리얼 복사
        _background.material = new(_background.material);

        //쉐이더 변수 ID 캐싱
        _backgroundColorPropertyID = Shader.PropertyToID(BACKGROUND_COLOR);
        _circleColorPropertyID = Shader.PropertyToID(CIRCLE_COLOR);
    }

    private void Start()
    {
        //게임 매니저 이벤트 등록
        if (GameManager.Instance)
        {
            GameManager.Instance.RegisterEvent(GameState.Round, OnRoundStarted);
        }
    }

    private void OnDestroy()
    {
        //게임 매니저 이벤트 해제
        if (GameManager.Instance)
        {
            GameManager.Instance.UnregisterEvent(GameState.Round, OnRoundStarted);
        }
    }

    private void OnRoundStarted()
    {
        //시퀀스 매니저를 통해 색 변경 애니메이션 등록
        SequenceManager.Instance.AddCoroutine(ChangeColor);
    }

    private void ChangeColor()
    {
        //기존 색상 저장
        Color originalBackgroundColor = _background.material.GetColor(_backgroundColorPropertyID);
        Color originalCircleColor = _background.material.GetColor(_circleColorPropertyID);

        //랜덤하게 Hue 값 결정
        float originalHue = GetHue(originalBackgroundColor);
        float randomHueDiff = Random.Range(0.25f, 0.75f);
        float randomHue = Mathf.Repeat(originalHue + randomHueDiff, 1f);

        //Hue 변경
        Color backgroundColor = ChangeColorHue(originalBackgroundColor, randomHue);
        Color circleColor = ChangeColorHue(originalCircleColor, randomHue);

        //트윈 애니메이션 실행
        _background.material.DOComplete();
        _background.material.DOColor(backgroundColor, _backgroundColorPropertyID, _duration);
        _background.material.DOColor(circleColor, _circleColorPropertyID, _duration);
    }

    //색상에서 Hue 값만 추출하는 함수
    private float GetHue(Color color)
    {
        Color.RGBToHSV(color, out float hue, out float _, out float _);
        return hue;
    }

    //색상에서 Hue만 변경하는 함수
    private Color ChangeColorHue(Color color, float hue)
    {
        Color.RGBToHSV(color, out float _, out float saturation, out float value);
        return Color.HSVToRGB(hue, saturation, value);
    }
}