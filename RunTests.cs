using testify;


class RunTests
{
    private static bool _done;
    private static int _exitCode;
    static int Main(string[] args)
    {
        RunTestsAsync();
        while(_done == false) {}

        return _exitCode;
    }

    static async void RunTestsAsync()
    {
        // run Single Test
        //_exitCode = await Tools.RunTestAsync(new ExamplePassingTests());
        
        // run all test that inherits ATestSuite --> uses reflection
        _exitCode = await Tools.RunTestsAsync();
        _done = true;
    }
}
