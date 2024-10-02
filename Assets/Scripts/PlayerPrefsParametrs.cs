public static class PlayerPrefsParametrs 
{
    public static string IsAdsRemove = nameof(IsAdsRemove);                             //  +   0 - default, 1 - после покупки рекламы
    public static string FirstStartForAd = nameof(FirstStartForAd);                               //  -   0 - default, 1 - после проверки на покупку ин-аппа
    public static string FirstStartForPers = nameof(FirstStartForPers);                               //  -   0 - default, 1 - после проверки на покупку ин-аппа
    public static string FirstGameStart = nameof(FirstGameStart);                       //  +   0 - default, 1 - после нажатия на кнопку выбора персонажа
    public static string IsAdsMan = nameof(IsAdsMan);                                   //  +   0 - default, 1 - после после покупки или выбора этого персонажа
    public static string IsAdsGirl = nameof(IsAdsGirl);                                 //  +   0 - default, 1 - после после покупки или выбора этого персонажа
    public static string StarsCounter = nameof(StarsCounter);                           //  +   0 - со старта и при перезагрузке сцены, увеличение по мере прохождения игры
    public static string DieCounter = nameof(DieCounter);                               //  +   0 - со старта и при перезагрузке сцены, увеличение по мере прохождения игры
    public static string CurrentDoorIndex = nameof(CurrentDoorIndex);         
    public static string CurrentNumberCheckpoint = nameof(CurrentNumberCheckpoint);     //  +   0 - со старта и при перезагрузке сцены, увеличение по мере прохождения игры
    public static string PrivacePolicyIsActive = nameof(PrivacePolicyIsActive);         //  -   0 - default, 1 - после нажатия на кноку соглашения с поликитой конфиденциальности
}