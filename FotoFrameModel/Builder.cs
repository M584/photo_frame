using Kompas6Constants3D;
using Kompas6LTAPI5;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace FotoFrameModel
{
    /// <summary>
    /// Предоставляет возможность строить модель
    ///     по параметрам фоторамки в САПР
    /// </summary>
    public interface IBuilder
    {
        /// <summary>
        /// Построить модель фоторамки в САПР
        /// </summary>
        /// <param name="photoFrame">Шаблон фоторамки</param>
        /// <param name="checker">Проверяющий параметры фоторамки</param>
        /// <exception cref="InvalidOperationException">
        ///     Вызывается тогда, когда параметры фоторамки
        ///     имеют недопустимые значения.</exception>
        void Build(IPhotoFrame photoFrame, IChecker checker);
    }


    /// <summary>
    /// Мастер по созданию фоторамок в САПР Компас 3D
    /// </summary>
    public class BuilderPhotoFrame : IBuilder
    {
        private KompasObject _kompas;

        private double _halfX;
        private double _halfY;

        public BuilderPhotoFrame()
        {
            //var progId = "KOMPASLT.Application.5";
            //Type t = Type.GetTypeFromProgID(progId);

            ////Получение ссылки на запущенную копию Компас 3д
            //_kompas = (KompasObject)Marshal.GetActiveObject(progId);
            //_kompas = null;
            //if (_kompas == null)
            //{
            //    //Так как нету запущенной копии, то запускаем Компас 3д сами
            //    _kompas = (KompasObject)Activator.CreateInstance(t);
            //}

            //if (_kompas != null)
            //{
            //    _kompas.Visible = true;
            //    _kompas.ActivateControllerAPI();
            //}
            //Возможно понадобиться выкидывать исключение, что не получили ссылку на Компас 3д
        }

        /// <summary>
        /// Построить модель фоторамки в САПР Компас 3D
        /// </summary>
        /// <param name="photoFrame">Шаблон фоторамки</param>
        /// <param name="checker">Проверяющий параметры фоторамки</param>
        /// <exception cref="InvalidOperationException">
        ///     Вызывается тогда, когда параметры фоторамки
        ///     имеют недопустимые значения.</exception>
        public void Build(IPhotoFrame photoFrame, IChecker checker)
        {
            if (checker.IsValid == false)
            {
                var msg = $"Шаблон фоторамки имеет" +
                    $" недопустимые параметры для построения.";
                throw new InvalidOperationException(msg);
            }

            _halfX = photoFrame.OuterHeight / 2.0f;
            _halfY = _halfX;

            var doc = (ksDocument3D)_kompas.Document3D();
            doc.Create();

            GenerateBlock(photoFrame);
        }

        private void GenerateBlock(IPhotoFrame photoFrame)
        {
            var doc = (ksDocument3D)_kompas.ActiveDocument3D();
            if (doc == null)
            {
                return;
            }

            var part = (ksPart)doc.GetPart((short)Part_Type.pTop_Part);
            if (part == null)
            {
                return;
            }
            var basePlane = (ksEntity)part.GetDefaultEntity(
                (short)Obj3dType.o3d_planeXOY);

            var sketch = (ksEntity)part.NewEntity((short)Obj3dType.o3d_sketch);
            if (sketch == null)
            {
                return;
            }
            sketch.name = "Эскиз бруска стороны по длине";

            var sketchDef = (ksSketchDefinition)sketch.GetDefinition();
            if (sketchDef == null)
            {
                return;
            }
            sketchDef.SetPlane(basePlane);
            sketch.Create();

            var draw = (ksDocument2D)sketchDef.BeginEdit();

            var h1 = photoFrame.OuterHeight;
            var h2 = photoFrame.InnerHeight;
            var h = photoFrame.Interval;
            var Ax = _halfX;
            var Ay = _halfY;


            sketchDef.EndEdit();

            var extr = (ksEntity)part.NewEntity(
                (short)Obj3dType.o3d_bossExtrusion);
            if (extr == null)
            {
                return;
            }
            extr.name = "Выдавить брусок";

            var extrDef = (ksBossExtrusionDefinition)extr.GetDefinition();
            extrDef.SetSketch(sketch);
            if (extrDef == null)
            {
                return;
            }

            extrDef.directionType = (short)Direction_Type.dtNormal;

            var depthExtrusion = photoFrame.OuterLength;
            var angle = 0.0f;
            extrDef.SetSideParam(true,
                                 (short)End_Type.etBlind,
                                 depthExtrusion,
                                 angle,
                                 false);
            extrDef.SetSketch(sketch);
            extr.Create();
        }
    }
}
