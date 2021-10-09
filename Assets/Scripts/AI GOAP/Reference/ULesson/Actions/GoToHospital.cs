public class GoToHospital : GAction {
    public override bool PrePerform() {

        return true;
    }

    public override bool PostPerform() {

        return true;
    }

    public override bool PostPerformCleanUp()
    {
        throw new System.NotImplementedException();
    }
}
