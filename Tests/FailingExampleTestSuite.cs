using System;
using testify;

public class FailingExampleTestSuite : ATestSuite
{
    public override void Run()
    {
        Describe("Fail suite - BasicTypes", () =>
        {
            It("should fail on Boolean comparison", (assert) => assert(true, false));
            It("should fail on int comparison", (assert) => assert(-1, 1));
            It("should fail on string comparison", (assert) => assert("foo", "bar"));
        });

        Describe("Excluded and NotImplemented suite - BasicTypes", () =>
        {
            XIt("demonstrate an example of excluded test", (assert) => assert(true, true));

            It("demonstrate an example of not implemented test");
        });

        var fakeSystemExceptionMessage = "My fake system error message";
        void FakeSystemFn()
        {
            if(false)
                throw new Exception(fakeSystemExceptionMessage);
        }

        Describe("Fail suite - Expected Exception", () =>
        {
            It("should fail to catch an expected Exception", (assert) => {
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
