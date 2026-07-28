using System;

namespace Amrit.SaveSystem
{
    public sealed class SaveManager
    {
        private readonly SaveRegistry registry;

        private readonly ISaveProvider provider;

        public SaveManager(
            SaveRegistry registry,
            ISaveProvider provider)
        {
            this.registry = registry;
            this.provider = provider;
        }

        public SaveResult Save(string key)
        {
            try
            {
                if (!registry.TryGet(key, out ISaveable saveable))
                {
                    return SaveResult.Fail(
                        $"'{key}' is not registered.");
                }

                provider.Save(
                    saveable.SaveKey,
                    saveable.CaptureState(),
                    saveable.StateType);

                return SaveResult.Ok();
            }
            catch (Exception ex)
            {
                return SaveResult.Fail(
                    ex.Message,
                    ex);
            }
        }

        public SaveResult Load(string key)
        {
            try
            {
                if (!registry.TryGet(key, out ISaveable saveable))
                {
                    return SaveResult.Fail(
                        $"'{key}' is not registered.");
                }

                if (!provider.Exists(key))
                {
                    return SaveResult.Fail(
                        $"Save '{key}' not found.");
                }

                object state =
                    provider.Load(
                        key,
                        saveable.StateType);

                saveable.RestoreState(state);

                return SaveResult.Ok();
            }
            catch (Exception ex)
            {
                return SaveResult.Fail(
                    ex.Message,
                    ex);
            }
        }

        public SaveResult SaveAll()
        {
            try
            {
                foreach (ISaveable saveable in registry.GetAll())
                {
                    provider.Save(
                        saveable.SaveKey,
                        saveable.CaptureState(),
                        saveable.StateType);
                }

                return SaveResult.Ok();
            }
            catch (Exception ex)
            {
                return SaveResult.Fail(
                    ex.Message,
                    ex);
            }
        }

        public SaveResult LoadAll()
        {
            try
            {
                foreach (ISaveable saveable in registry.GetAll())
                {
                    if (!provider.Exists(saveable.SaveKey))
                    {
                        continue;
                    }

                    object state =
                        provider.Load(
                            saveable.SaveKey,
                            saveable.StateType);

                    saveable.RestoreState(state);
                }

                return SaveResult.Ok();
            }
            catch (Exception ex)
            {
                return SaveResult.Fail(
                    ex.Message,
                    ex);
            }
        }

        public void Delete(string key)
        {
            provider.Delete(key);
        }

        public void DeleteAll()
        {
            provider.DeleteAll();
        }
    }
}