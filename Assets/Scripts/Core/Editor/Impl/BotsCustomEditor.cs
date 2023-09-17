using System.Collections.Generic;
using Features.Bots.Impl;
using Modules.GameController.Data;
using Modules.GameController.Data.Impl;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Impl
{
    public class BotsCustomEditor : CustomEditors
    {
        private const string Path = "Assets/ScriptableObjects/BotsData.asset";
        
        private BotsScriptableObject _botsScriptableObject;
        private Vector2 _scrollPosition = Vector2.zero;
        private bool _isShowBots = true;

        [MenuItem("Window/Edit Bots")]
        private static void Init()
        {
            GetWindow(typeof(BotsCustomEditor));
        }

        private void OnEnable()
        {
            _botsScriptableObject = AssetDatabase.LoadAssetAtPath(Path, typeof(BotsScriptableObject)) as BotsScriptableObject;
        }

        private void OnGUI()
        {
            var botsInEditor = new List<BotTo>();

            if (GUILayout.Button("Add Bot"))
            {
                AddBot();
            }

            if (_botsScriptableObject.Bots.Count > 0)
            {
                botsInEditor.AddRange(_botsScriptableObject.Bots);
            }

            _isShowBots = EditorGUILayout.Foldout(_isShowBots, "Bots: " + _botsScriptableObject.Bots.Count);
            EditorGUI.indentLevel = 1;
            if (_isShowBots)
            {
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, true);

                for (var i = 0; i < botsInEditor.Count; i++)
                {
                    EditorGUI.indentLevel += 1;
                    HorizontalLine(Color.red);
                    var bot = botsInEditor[i];

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Bot:  " + bot.BotConfig.BotId);
                    if (GUILayout.Button("Remove Bot"))
                    {
                        RemoveBot(i);
                        continue;
                    }

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    bot.BotConfig.BotId = EditorGUILayout.TextField("ID", bot.BotConfig.BotId);
                    bot.BotConfig.MaxCount = EditorGUILayout.IntField("MaxCount", bot.BotConfig.MaxCount);
                    bot.BotConfig.Reward = EditorGUILayout.IntField("Reward", bot.BotConfig.Reward);
                    bot.BotConfig.SpawnDelay = EditorGUILayout.FloatField("SpawnDelay", bot.BotConfig.SpawnDelay);
                    bot.BotPrefab = EditorGUILayout.ObjectField(bot.BotPrefab, typeof(Bot), false) as Bot;
                    
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel -= 1;

                    EditorGUILayout.Space(50);

                    if (botsInEditor.Count > 0)
                    {
                        botsInEditor[i] = bot;
                    }
                }

                GUILayout.EndScrollView();
            }

            _botsScriptableObject.Bots.Clear();
            _botsScriptableObject.Bots.AddRange(botsInEditor);
            EditorGUI.indentLevel = 0;

            if (GUI.changed)
                EditorUtility.SetDirty(_botsScriptableObject);
            
            void RemoveBot(int index)
            {
                botsInEditor.RemoveAt(index);
            }

            void AddBot()
            {
                var bot = new BotTo();
                botsInEditor.Add(bot);
                bot.BotConfig.Reward = 1;
            }
        }
    }
}