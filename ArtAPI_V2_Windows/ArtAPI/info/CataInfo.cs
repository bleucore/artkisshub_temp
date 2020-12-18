using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.info
{
    public  class   CataInfo
    {
        public  Artist  artist;

        public  string  issue_id;

        public  string  title;
        public  string  thumb;

        public  string  artist_intro;
        public  string  artist_note;

        public  string  introduction;
        public  string  private_exhib;
        public  string  group_exhib;

        public  List<Artwork>   mArtworks   = new List<Artwork>();

        public  CataInfo(string artist_id)
        {
            artist  = new Artist(artist_id);
        }


        public  void    MakeTestData()
        {
            artist.localname    = "이수명";
            artist.englishname   = "Soomyoung Yi";

            artist.email    = "bluecor0@gmail.com";
            artist.instagram = "@bluecor0";
            artist.facebook = "https://fb.me/bluecor0";
            artist.blog     = "https://my.blog.com";
            artist.homepage = "https://my.homepage.com";


            title   = "팀 작품 도록집";
            thumb   = "https://image.issuu.com/200914191952-9047244b8650d80eaaf5e868c3cfdc31/jpg/page_1_thumb_large.jpg";

            artist_intro    = "asdfasdfasdfasdf\nasdfasdfasdf\n";
            artist_note = "1234567asdfasdfasdfasdf123456789012123456789012345678901234567890123456789012345678901234567890123456789012345678901234567893456789123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789asdfasdfasdfasdf890\nasdfasdfasdf\n1234567890\nasdfasdfasdf\n1234567890\nasdfasdfasdf\n1234567890\nasdfasdfasdf\n1234567890\nasdfasdfasdf\n1234567890\nasdfasdfasdf\n";
            //artist_note = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";

            introduction    = "introduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\n";
            private_exhib   = "private_exhib----1234567890\nasdfasdfasdf\nprivate_exhib----1234567890\nasdfasdfasdf\nprivate_exhib----1234567890\nasdfasdfasdf\nprivate_exhib----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\n";
            group_exhib     = "group_exhib----1234567890\nasdfasdfasdf\ngroup_exhib----1234567890\nasdfasdfasdf\ngroup_exhib----1234567890\nasdfasdfasdf\ngroup_exhib----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\nintroduction----1234567890\nasdfasdfasdf\n";


            mArtworks.Clear();
            for (int idx = 0; idx < 3; idx++)
            {
                Artwork artwork = new Artwork();
                artwork.image   = "https://image.issuu.com/200914191952-9047244b8650d80eaaf5e868c3cfdc31/jpg/page_1_thumb_large.jpg";

                artwork.title   = string.Format("작품 {0}", idx + 1);
                artwork.size    = string.Format("{0} x {1}", idx + 1, idx + 3);
                artwork.year    = string.Format("{0}", idx + 1 + 2000);
                artwork.material= string.Format("재료 {0}", idx + 1);

                mArtworks.Add(artwork);
            }
        }

    }
}
