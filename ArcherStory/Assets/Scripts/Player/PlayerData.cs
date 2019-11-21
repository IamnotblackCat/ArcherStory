/****************************************************
    文件：Playerdata.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/11/20 10:22:49
	功能：玩家数据相关
*****************************************************/

using UnityEngine;

public class PlayerData 
{
    public int hp;
    public int lv;
    public int exp;
    public int attackValue;
    public int defend;
    public int critical;
    public int pierce;
    public int coin;
    public int crystal;

    //public PlayerData()
    //{
    //    hp = 475;
    //    lv = 1;
    //    exp = 0;
    //    attackValue = 70;
    //    defend = 15;
    //    critical = 5;
    //    pierce = 5;
    //    coin = 1000;
    //    crystal = 1000;
    //}
    public void SaveJson()
    {
        Memento memento = new Memento();
        memento.SaveByJson();
    }
    public void ReadJson()
    {
        Memento memento = new Memento();
        PlayerData pd= memento.ReadByJsonFile();
        hp = pd.hp;
        lv = pd.lv;
        exp = pd.exp;
        attackValue = pd.attackValue;
        defend = pd.defend;
        critical = pd.critical;
        pierce = pd.pierce;
        coin = pd.coin;
        critical = pd.critical;
    }
    public void CalculateExp(int addExp)
    {
        int currentExp = exp;
        int curLv = lv;
        int addExpValue = addExp;
        while (true)
        {
            int upNeedExp = 100 - currentExp;
            if (addExpValue>upNeedExp)
            {
                curLv++;
                //TODO：升级特效
                addExpValue -= upNeedExp;
                currentExp = 0;
            }
            else
            {
                lv = curLv;
                exp = currentExp + addExpValue;
                break;
            }
        }
        SaveJson();
    }
}