#if DEBUG_MODE
using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    [ExecuteInEditMode]
    public class TestGUIController : TestBehaviour
    {
        public class Test
        {
            static readonly float TestLabelWidth = 100;
            static readonly float TestValueWidth = 200;

            public enum TestType
            {
                Test1,
                Test2,
                Test3,
            }

            IGUIController<bool> _boolGUIController = GUIController.CreateBool("Bool");
            IGUIController<bool> _boolGUIController2 = GUIController.CreateBool("Bool2", true).SetTooltip("Test Bool").SetLabelWidth(TestLabelWidth).SetValueWidth(TestValueWidth);
            IGUIController<int> _intGUIController = GUIController.CreateInt("Int");
            IGUIController<int> _intGUIController2 = GUIController.CreateInt("Int2", 123).SetTooltip("Test Int").SetLabelWidth(TestLabelWidth).SetValueWidth(TestValueWidth);
            IGUIController<float> _floatGUIController = GUIController.CreateFloat("Float");
            IGUIController<float> _floatGUIController2 = GUIController.CreateFloat("Float2", 1.23f).SetTooltip("Test Float").SetLabelWidth(TestLabelWidth).SetValueWidth(TestValueWidth);
            IGUIController<string> _stringGUIController = GUIController.CreateString("String");
            IGUIController<string> _stringGUIController2 = GUIController.CreateString("String2", "123").SetTooltip("Test String").SetLabelWidth(TestLabelWidth).SetValueWidth(TestValueWidth);
            IGUIController<TestType> _enumGUIController = GUIController.CreateEnum<TestType>("Enum");
            IGUIController<TestType> _enumGUIController2 = GUIController.CreateEnum("Enum2", TestType.Test2).SetTooltip("Test Enum").SetLabelWidth(TestLabelWidth).SetValueWidth(TestValueWidth);
            IGUIController<Color> _colorGUIController = GUIController.CreateColor("Color");
            IGUIController<Color> _colorGUIController2 = GUIController.CreateColor("Color2", Color.green).SetTooltip("Test Color").SetLabelWidth(TestLabelWidth).SetValueWidth(TestValueWidth);
            IGUIController<Color> _colorGUIController3 = GUIController.CreateSliderColor("Color3");
            IGUIController<Color> _colorGUIController4 = GUIController.CreateSliderColor("Color4", Color.red).SetTooltip("Test Slider Color").SetLabelWidth(TestLabelWidth).SetValueWidth(TestValueWidth);
            IGUIController<Rect> _rectGUIController = GUIController.CreateRect("Rect");
            IGUIController<Rect> _rectGUIController2 = GUIController.CreateRect("Rect2", new Rect(0, 0, 1, 1)).SetTooltip("Test Rect").SetLabelWidth(TestLabelWidth).SetValueWidth(TestValueWidth);
            IGUIController<SystemLanguage> _languageGUIController = GUIController.CreateLanguage("Language");
            IGUIController<SystemLanguage> _languageGUIController2 = GUIController.CreateLanguage("Language2", SystemLanguage.Vietnamese).SetTooltip("Test Language").SetLabelWidth(TestLabelWidth).SetValueWidth(TestValueWidth);

            List<IBaseGUIController> _guiControllers;
            public List<IBaseGUIController> GUIControllers => _guiControllers ??= new()
            {
                _boolGUIController,
                _boolGUIController2,
                _intGUIController,
                _intGUIController2,
                _floatGUIController,
                _floatGUIController2,
                _stringGUIController,
                _stringGUIController2,
                _enumGUIController,
                _enumGUIController2,
                _colorGUIController,
                _colorGUIController2,
                _colorGUIController3,
                _colorGUIController4,
                _rectGUIController,
                _rectGUIController2,
                _languageGUIController,
                _languageGUIController2,
            };

            public void OnGUI()
            {
                foreach (var controller in GUIControllers)
                {
                    controller.OnGUI();
                }

                GUIHelper.Line();
                GUIHelper.LayoutCenter(() =>
                {
                    if (GUIHelper.Button("Log"))
                    {
                        foreach (var controller in GUIControllers)
                        {
                           LegacyLog.Debug(controller);
                        }
                    }
                });
                GUIHelper.Line();
            }
        }

        Test _test = new();

        public override void OnSceneGUI()
        {
            GUIHelper.SetSceneStyle(_test.GUIControllers);
            _test.OnGUI();
        }

        void OnGUI()
        {
            _test.OnGUI();
        }
    }
}
#endif