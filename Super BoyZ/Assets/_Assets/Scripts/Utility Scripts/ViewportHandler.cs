using UnityEngine;


namespace GamerWolf.Utils {
    [RequireComponent (typeof (Camera))]
    [ExecuteInEditMode]
    public class ViewportHandler : MonoBehaviour {
    #region FIELDS
        [SerializeField] private Color wireColor = Color.white;
        [SerializeField] private float UnitsSize = 1; // size of your scene in unity units
        [SerializeField] private float bgSize = 1f;
        [SerializeField] private Constraint constraint = Constraint.Portrait;
        [SerializeField] private Transform bg;

        private Camera m_camera;
        private float _width;
        private float _height;
        //*** bottom screen
        private Vector3 _bl;
        private Vector3 _bc;
        private Vector3 _br;
        //*** middle screen
        private Vector3 _ml;
        private Vector3 _mc;
        private Vector3 _mr;
        //*** top screen
        private Vector3 _tl;
        private Vector3 _tc;
        private Vector3 _tr;
        #endregion

        #region PROPERTIES
        public float Width {
            get {
                return _width;
            }
        }
        public float Height {
            get {
                return _height;
            }
        }

        // helper points:
        public Vector3 BottomLeft {
            get {
                return _bl;
            }
        }
        public Vector3 BottomCenter {
            get {
                return _bc;
            }
        }
        public Vector3 BottomRight {
            get {
                return _br;
            }
        }
        public Vector3 MiddleLeft {
            get {
                return _ml;
            }
        }
        public Vector3 MiddleCenter {
            get {
                return _mc;
            }
        }
        public Vector3 MiddleRight {
            get {
                return _mr;
            }
        }
        public Vector3 TopLeft {
            get {
                return _tl;
            }
        }
        public Vector3 TopCenter {
            get {
                return _tc;
            }
        }
        public Vector3 TopRight {
            get {
                return _tr;
            }
        }
    #endregion

    #region METHODS
        public static ViewportHandler i;
        private void Awake(){
            if(i == null){
                i = this;
            }else{
                Destroy(i.gameObject);
            }

            m_camera = GetComponent<Camera>();
            ComputeResolution();
        }
        

        private void ComputeResolution(){
            float leftX, rightX, topY, bottomY;

            if(constraint == Constraint.Landscape){
                
                m_camera.orthographicSize = 1f / m_camera.aspect * UnitsSize / 2f;    
                if(bg != null){
                    bg.localScale = new Vector3(bgSize/2f,bgSize/2f,1f);
                }
            }else{
                m_camera.orthographicSize = UnitsSize / 2f;
                if(bg != null){
                    bg.localScale = new Vector3(bgSize/2f,bgSize/2f,1f);
                }
            }

            _height = 2f * m_camera.orthographicSize;
            _width = _height * m_camera.aspect;

            float cameraX, cameraY;
            cameraX = m_camera.transform.position.x;
            cameraY = m_camera.transform.position.y;

            leftX = cameraX - _width / 2;
            rightX = cameraX + _width / 2;
            topY = cameraY + _height / 2;
            bottomY = cameraY - _height / 2;

            //*** bottom
            _bl = new Vector3(leftX, bottomY, 0);
            _bc = new Vector3(cameraX, bottomY, 0);
            _br = new Vector3(rightX, bottomY, 0);
            //*** middle
            _ml = new Vector3(leftX, cameraY, 0);
            _mc = new Vector3(cameraX, cameraY, 0);
            _mr = new Vector3(rightX, cameraY, 0);
            //*** top
            _tl = new Vector3(leftX, topY, 0);
            _tc = new Vector3(cameraX, topY , 0);
            _tr = new Vector3(rightX, topY, 0);           
        }

        private void Update(){
            ComputeResolution();
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = wireColor;

            Matrix4x4 temp = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            if (m_camera.orthographic) {
                float spread = m_camera.farClipPlane - m_camera.nearClipPlane;
                float center = (m_camera.farClipPlane + m_camera.nearClipPlane)*0.5f;
                Gizmos.DrawWireCube(new Vector3(0,0,center), new Vector3(m_camera.orthographicSize*2*m_camera.aspect, m_camera.orthographicSize*2, spread));
            } else {
                Gizmos.DrawFrustum(Vector3.zero, m_camera.fieldOfView, m_camera.farClipPlane, m_camera.nearClipPlane, m_camera.aspect);
            }
            Gizmos.matrix = temp;
        }
        #endregion

        public enum Constraint { Landscape, Portrait }
    }
}