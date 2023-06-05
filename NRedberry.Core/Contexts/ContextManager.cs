using System;
using NRedberry.Core.Contexts.Defaults;

namespace NRedberry.Core.Contexts;

/**
     * This class implements context management logic.
     *
     * <p>It holds current thread-local context of Redberry session (see description for {@link Context} class).
     * It is possible to set context explicitly using {@link #setCurrentContext(Context)} method.
     * Each thread is linked to its own context. All child threads created via {@code ExecutorService}
     * from {@link #getExecutorService()} have same context.</p>
     */
public static class ContextManager
{
    /**
         * Thread-local container for the current context
         */
    [ThreadStatic]
    private static Context ThreadLocalContext = new Context(new ContextSettings());

    public static Context CurrentContext { get; set; }

    /**
         * Returns the current context of Redberry session.
         *
         * @return the current context of Redberry session.
         */
    public static Context GetCurrentContext()
    {
        return ThreadLocalContext;
    }

    /**
         * This method initializes and sets current session context by the default
         * value defined in {@link DefaultContextFactory}. After this step, all
         * tensors that exist in the thread will be invalidated.
         *
         * @return created context
         */
    public static Context InitializeNew()
    {
        ThreadLocalContext = DefaultContextFactory.Instance.CreateContext();
        return ThreadLocalContext;
    }

    /**
         * This method initializes and sets current session context from
         * the specified {@code context settings} ({@link ContextSettings}).
         * After invocation of this method, all the tensors that exist in
         * the current thread will be invalidated.
         *
         * @return created context
         */
    public static Context InitializeNew(ContextSettings contextSettings)
    {
        var context = new Context(contextSettings);
        ThreadLocalContext = context;
        return context;
    }

    /**
         * Sets current thread-local context to the specified one. After this step, all the
         * tensors that exist in the thread will be invalidated.
         *
         * @param context context
         */
    public static void SetCurrentContext(Context context)
    {
        ThreadLocalContext = context;
    }
}