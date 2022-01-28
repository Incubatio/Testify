using testify;

public class ExamplePassingTests : ATestSuite
{
    public override void Run()
    {
        Describe("Pass suite - BasicTypes", () =>
        {
            It("should pass on Boolean comparison", (assert) => assert(true, true));
            It("should pass on int comparison", (assert) => assert(1, 1));
            It("should pass on string comparison", (assert) => assert("foo", "foo"));
        });
    }
    
}