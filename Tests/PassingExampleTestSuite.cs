using System;
using testify;

public class PassingExampleTestSuite : ATestSuite
{
    public override void Run()
    {
        Describe("Pass suite - BasicTypes", () =>
        {
            It("should pass on Boolean comparison", (assert) => assert(true, true));
            It("should pass on int comparison", (assert) => assert(1, 1));
            It("should pass on string comparison", (assert) => assert("foo", "foo"));
        });

        var fakeSystemExceptionMessage = "My fake system error message";
        void FakeSystemFn() =>
            throw new Exception(fakeSystemExceptionMessage);

        Describe("Pass suite - Expected Exception", () =>
        {
            It("", (assert) => {
                try {
                    FakeSystemFn();
                    throw new Exception(EXPECTED_EXCEPTION);
                }
                catch(Exception e)
                {
                    assert(e.Message, fakeSystemExceptionMessage);
                }
            });
        });


    }

}
