using testify;

public class ExampleOtherTests : ATestSuite
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
    }

}