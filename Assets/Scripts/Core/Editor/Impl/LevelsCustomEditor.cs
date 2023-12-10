using System.Collections.Generic;
using Features.Bots.Impl;
using Modules.GameController.Data;
using Modules.GameController.Data.Impl;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Impl
{
    // TODO: use ODIN or smth else
    public class LevelsCustomEditor : CustomEditors
    {
        private const string Path = "Assets/ScriptableObjects/LevelsData.asset";

        private LevelsRepository _levelsRepository;
        private Vector2 _scrollPosition = Vector2.zero;
        private bool _isShowLevels = true;

        [MenuItem("Window/Edit Levels")]
        private static void Init()
        {
            GetWindow(typeof(LevelsCustomEditor));
        }

        private void OnEnable()
        {
            _levelsRepository = AssetDatabase.LoadAssetAtPath(Path, typeof(LevelsRepository)) as LevelsRepository;
        }

        private void OnGUI()
        {
            var levelsInEditor = new List<LevelTo>();

            if (GUILayout.Button("Add Level"))
            {
                AddLevel();
            }

            if (_levelsRepository.Levels.Count > 0)
            {
                levelsInEditor.AddRange(_levelsRepository.Levels);
            }

            _isShowLevels = EditorGUILayout.Foldout(_isShowLevels, "Levels: " + _levelsRepository.Levels.Count);
            EditorGUI.indentLevel = 1;
            if (_isShowLevels)
            {
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, true);

                for (var i = 0; i < levelsInEditor.Count; i++)
                {
                    EditorGUI.indentLevel += 1;
                    HorizontalLine(Color.red);
                    var level = levelsInEditor[i];

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Level # {level.LevelId}");
                    if (GUILayout.Button("Remove Level"))
                    {
                        RemoveLevel(i);
                        continue;
                    }

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    level.LevelId = EditorGUILayout.IntField("ID: ", level.LevelId);
                    level.PointsToFinish = EditorGUILayout.IntField("PointsToFinish: ", level.PointsToFinish);
                    level.Bots ??= new List<BotConfig>();

                    if (GUILayout.Button("Remove Level"))
                    {
                        level.Bots.Add(new BotConfig());
                        continue;
                    }
                    
                    EditorGUILayout.EndHorizontal();

                    foreach (var botConfig in level.Bots)
                    {
                        EditorGUI.indentLevel += 1;
                        HorizontalLine(Color.red);

                        EditorGUILayout.LabelField($"Bot # {(botConfig.Prefab == null ? "NONE" : botConfig.Prefab.ID)}");
                        botConfig.Prefab = EditorGUILayout.ObjectField(botConfig.Prefab, typeof(Bot), false) as Bot;
                        botConfig.MaxCount = EditorGUILayout.IntField("MaxCount: ", botConfig.MaxCount);
                        botConfig.SpawnDelay = EditorGUILayout.FloatField("SpawnDelay: ", botConfig.SpawnDelay);
                        EditorGUI.indentLevel -= 1;
                    }

                    EditorGUILayout.Space(50);

                    if (levelsInEditor.Count > 0)
                    {
                        levelsInEditor[i] = level;
                    }
                }

                GUILayout.EndScrollView();
            }

            _levelsRepository.Levels.Clear();
            _levelsRepository.Levels.AddRange(levelsInEditor);
            EditorGUI.indentLevel = 0;

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_levelsRepository);
            }

            void RemoveLevel(int index)
            {
                levelsInEditor.RemoveAt(index);
            }

            void AddLevel()
            {
                var level = new LevelTo();
                levelsInEditor.Add(level);
            }
        }
    }
}