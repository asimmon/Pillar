
using System;
using System.Linq.Expressions;
using Askaiser.Mobile.Pillar.ViewModels;

namespace Askaiser.Mobile.Pillar.Tests.Mocks
{
    public class MockViewModel : PillarViewModelBase
    {
        private string _lambdaStringProperty;
        public string LambdaStringProperty
        {
            get { return _lambdaStringProperty; }
            set { Set(() => LambdaStringProperty, ref _lambdaStringProperty, value); }
        }

        private string _withoutLambdaStringProperty;
        public string WithoutLambdaStringProperty
        {
            get
            {
                return _withoutLambdaStringProperty;
            }
            set { Set(ref _withoutLambdaStringProperty, value); }
        }

        private string _emptyLambdaStringProperty;
        public string EmptyLambdaStringProperty
        {
            set { Set(() => null, ref _emptyLambdaStringProperty, value); }
        }

        private string _nullLambdaStringProperty;
        public string NullLambdaStringProperty
        {
            set { Set((Expression<Func<string>>) null, ref _nullLambdaStringProperty, value); }
        }

        private string _notALambdaProperty;
        public string NotALambdaProperty
        {
            set { Set(() => _notALambdaProperty, ref _notALambdaProperty, value); }
        }
    }
}
