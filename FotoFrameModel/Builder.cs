using System;

namespace FotoFrameModel
{
    public interface IBuilder
    {
        void Build(IPhotoFrame photoFrame, IChecker checker);
    }


    public class BuilderPhotoFrame : IBuilder
    {
        public void Build(IPhotoFrame photoFrame, IChecker checker)
        {
            throw new NotImplementedException();
        }
    }
}
