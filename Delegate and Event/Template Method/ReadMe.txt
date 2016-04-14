
***** 이벤트 사용 방법 *****

1. 이벤트에서 사용할 대리자(델리게이트) 추가 또는 제너릭 EventHandler을 사용.
   public delegate void AgeChangedEventHandler( object sender, AgeChangedEventArgs e );

 2. EventArgs를 상속하는 이벤트 인자 클래스를 만든다.
    public class AgeChangedEventArgs : EventArgs
    {
    }	

3. 이벤트를 선언(제너릭 형식으로 캡슐화)
   public event EventHandler<AgeChangedEventArgs> AgeChanged;
   // 제너릭이 아닌 이벤트 선언
   // public event AgeChangedEventArgs AgeChanged;

4. 이벤트를 호출하는 가상 메서드 작성
   protected virtual void OnAgeChanged(AgeChangedEventArgs e)
   {
      if (AgeChanged != null)
          AgeChanged( this, e );
   }
   public void IncreaseAge()
   {
      int oldAge = _age;
      _age++;
      OnAgeChanged( new AgeChangedEventArgs( oldAge, _age ) );
    }

5. 다른 클래스에서 가상 메서드 재정의(오버라이드)
   public class Entertainer : Person
    {
        protected override void OnAgeChanged( AgeChangedEventArgs e )
        {
            base.OnAgeChanged(new AgeChangedEventArgs( e.OldAge, e.OldAge) );
        }  
	}